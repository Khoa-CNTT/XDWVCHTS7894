document.addEventListener('DOMContentLoaded', function () {
    const goalListContainer = document.getElementById('goalListContainer');
    const btnCreateGoal = document.getElementById('btnCreateGoal');
    const goalModalElement = document.getElementById('goalModal');
    const goalModal = goalModalElement ? new bootstrap.Modal(goalModalElement) : null; // Khởi tạo Bootstrap Modal
    const goalForm = document.getElementById('goalForm');
    const saveGoalButton = document.getElementById('saveGoalButton');

    // Input fields trong modal
    const goalIdInput = document.getElementById('goalIdInput');
    const goalTitleInput = document.getElementById('goalTitle');
    const goalDescriptionInput = document.getElementById('goalDescription');
    const goalTargetDateInput = document.getElementById('goalTargetDate');
    const goalTargetValueInput = document.getElementById('goalTargetValue');
    const goalUnitInput = document.getElementById('goalUnit');
    const goalCurrentValueInput = document.getElementById('goalCurrentValue');
    const goalStatusInput = document.getElementById('goalStatus');
    const goalModalLabel = document.getElementById('goalModalLabel');

    const API_GOALS_URL = '/api/goals'; // Điều chỉnh nếu API của bạn có prefix khác

    // --- 1. Load Goals ---
    async function loadGoals() {
        if (!goalListContainer) return;
        goalListContainer.innerHTML = '<p>Đang tải mục tiêu...</p>'; // Loading state

        try {
            const response = await fetch(API_GOALS_URL);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const goals = await response.json();
            renderGoals(goals);
        } catch (error) {
            console.error('Lỗi khi tải mục tiêu:', error);
            goalListContainer.innerHTML = '<p class="text-danger">Không thể tải danh sách mục tiêu.</p>';
        }
    }

    function renderGoals(goals) {
        if (!goalListContainer) return;
        goalListContainer.innerHTML = ''; // Clear previous goals

        if (!goals || goals.length === 0) {
            goalListContainer.innerHTML = '<p>Bạn chưa có mục tiêu nào. Hãy tạo một mục tiêu mới!</p>';
            return;
        }

        goals.forEach(goal => {
            const targetDate = goal.targetDate ? new Date(goal.targetDate).toLocaleDateString() : 'Không có';
            const startDate = new Date(goal.startDate).toLocaleDateString();
            let progressPercent = 0;
            if (goal.targetValue && goal.targetValue > 0 && goal.currentValue) {
                progressPercent = Math.min(100, Math.max(0, (goal.currentValue / goal.targetValue) * 100));
            } else if (goal.status === 'Achieved') {
                progressPercent = 100;
            }


            const goalCard = `
                <div class="col-md-6 col-lg-4 mb-4">
                    <div class="card h-100">
                        <div class="card-body d-flex flex-column">
                            <h5 class="card-title">${escapeHtml(goal.title)}</h5>
                            <p class="card-text flex-grow-1">${escapeHtml(goal.description || 'Không có mô tả')}</p>
                            <p class="card-text"><small class="text-muted">Bắt đầu: ${startDate}</small></p>
                            <p class="card-text"><small class="text-muted">Mục tiêu đến: ${targetDate}</small></p>
                            ${goal.targetValue ? `
                                <p class="card-text"><small class="text-muted">
                                    Tiến độ: ${goal.currentValue || 0} / ${goal.targetValue} ${goal.unit || ''}
                                </small></p>
                                <div class="progress mb-2" style="height: 20px;">
                                    <div class="progress-bar bg-success" role="progressbar" style="width: ${progressPercent.toFixed(0)}%;"
                                         aria-valuenow="${progressPercent.toFixed(0)}" aria-valuemin="0" aria-valuemax="100">${progressPercent.toFixed(0)}%</div>
                                </div>
                            ` : ''}
                            <p class="card-text"><small>Trạng thái: <span class="fw-bold">${getGoalStatusText(goal.status)}</span></small></p>
                            <div class="mt-auto pt-2">
                                <button class="btn btn-sm btn-info btn-edit-goal" data-id="${goal.id}"><i class="bi bi-pencil-square"></i> Sửa</button>
                                <button class="btn btn-sm btn-danger btn-delete-goal" data-id="${goal.id}"><i class="bi bi-trash"></i> Xóa</button>
                                ${goal.targetValue ? `<button class="btn btn-sm btn-success btn-update-progress" data-id="${goal.id}" data-current="${goal.currentValue || 0}" data-target="${goal.targetValue}" data-unit="${goal.unit || ''}"><i class="bi bi-graph-up"></i> Cập nhật Tiến độ</button>` : ''}
                            </div>
                        </div>
                    </div>
                </div>
            `;
            goalListContainer.insertAdjacentHTML('beforeend', goalCard);
        });
    }
    function getGoalStatusText(status) {
        switch (status) {
            case 'InProgress': return 'Đang tiến hành';
            case 'Achieved': return 'Đã đạt được';
            case 'Paused': return 'Tạm dừng';
            case 'Abandoned': return 'Đã từ bỏ';
            default: return status;
        }
    }
    function escapeHtml(unsafe) {
        if (unsafe === null || typeof unsafe === 'undefined') return '';
        return unsafe
            .toString()
            .replace(/&/g, "&")   // Đúng: & thành &
            .replace(/</g, "<")    // Đúng: < thành <
            .replace(/>/g, ">")    // Đúng: > thành >
            .replace(/`/g, "`")  // Đúng: ` thành `
                .replace(/'/g, "'"); // Đúng: ' thành ' (hoặc ')
    }


    // --- 2. Xử lý sự kiện cho #btnCreateGoal ---
    if (btnCreateGoal) {
        btnCreateGoal.addEventListener('click', function () {
            if (!goalModal || !goalForm) return;
            goalForm.reset(); // Xóa trống form
            goalIdInput.value = ''; // Đảm bảo ID trống cho việc tạo mới
            goalModalLabel.textContent = 'Tạo Mục tiêu mới';
            goalCurrentValueInput.parentElement.style.display = 'none'; // Ẩn current value khi tạo mới
            goalStatusInput.parentElement.style.display = 'none'; // Ẩn status khi tạo mới
            goalModal.show();
        });
    }

    // --- 3. Xử lý sự kiện cho các nút .btn-edit-goal ---
    goalListContainer.addEventListener('click', async function (event) {
        if (event.target.classList.contains('btn-edit-goal')) {
            if (!goalModal || !goalForm) return;
            const goalId = event.target.dataset.id;
            try {
                const response = await fetch(`${API_GOALS_URL}/${goalId}`);
                if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
                const goal = await response.json();

                // Điền dữ liệu vào form
                goalForm.reset();
                goalIdInput.value = goal.id;
                goalTitleInput.value = goal.title;
                goalDescriptionInput.value = goal.description || '';
                goalTargetDateInput.value = goal.targetDate ? goal.targetDate.substring(0, 10) : ''; // Format YYYY-MM-DD
                goalTargetValueInput.value = goal.targetValue || '';
                goalUnitInput.value = goal.unit || '';
                goalCurrentValueInput.value = goal.currentValue || '';
                goalStatusInput.value = goal.status;


                goalModalLabel.textContent = 'Sửa Mục tiêu';
                goalCurrentValueInput.parentElement.style.display = 'block'; // Hiện current value khi sửa
                goalStatusInput.parentElement.style.display = 'block'; // Hiện status khi sửa
                goalModal.show();
            } catch (error) {
                console.error('Lỗi khi lấy chi tiết mục tiêu:', error);
                alert('Không thể tải thông tin mục tiêu để sửa.');
            }
        }
    });

    // --- 4. Xử lý sự kiện cho #saveGoalButton ---
    if (saveGoalButton) {
        saveGoalButton.addEventListener('click', async function () {
            if (!goalForm.checkValidity()) {
                goalForm.reportValidity(); // Hiển thị lỗi validation của trình duyệt nếu có
                return;
            }

            const goalData = {
                title: goalTitleInput.value,
                description: goalDescriptionInput.value,
                targetDate: goalTargetDateInput.value ? goalTargetDateInput.value : null,
                targetValue: goalTargetValueInput.value ? parseFloat(goalTargetValueInput.value) : null,
                unit: goalUnitInput.value,
                // Chỉ gửi currentValue và status nếu là edit
                currentValue: goalIdInput.value && goalCurrentValueInput.value ? parseFloat(goalCurrentValueInput.value) : null,
                status: goalIdInput.value ? goalStatusInput.value : null,
            };

            // Loại bỏ các trường null hoặc rỗng không cần thiết cho API (tùy vào API của bạn)
            if (!goalData.description) delete goalData.description;
            if (!goalData.targetDate) delete goalData.targetDate;
            if (goalData.targetValue === null || isNaN(goalData.targetValue)) delete goalData.targetValue;
            if (!goalData.unit) delete goalData.unit;
            if (goalData.currentValue === null || isNaN(goalData.currentValue)) delete goalData.currentValue;
            if (!goalData.status && goalIdInput.value) delete goalData.status; // Chỉ gửi status nếu đang edit và có giá trị


            const goalId = goalIdInput.value;
            const method = goalId ? 'PUT' : 'POST';
            const url = goalId ? `${API_GOALS_URL}/${goalId}` : API_GOALS_URL;

            try {
                const response = await fetch(url, {
                    method: method,
                    headers: {
                        'Content-Type': 'application/json',
                        // Thêm CSRF token header nếu cần
                    },
                    body: JSON.stringify(goalData)
                });

                if (!response.ok) {
                    const errorData = await response.json().catch(() => ({ message: `Lỗi ${response.status}` }));
                    throw new Error(errorData.message || `HTTP error! status: ${response.status}`);
                }

                goalModal.hide();
                loadGoals(); // Tải lại danh sách
                // Hiển thị thông báo thành công (ví dụ: toastr, SweetAlert)
                alert(goalId ? 'Mục tiêu đã được cập nhật!' : 'Mục tiêu đã được tạo mới!');

            } catch (error) {
                console.error('Lỗi khi lưu mục tiêu:', error);
                alert(`Không thể lưu mục tiêu: ${error.message}`);
            }
        });
    }

    // --- 5. Xử lý sự kiện cho các nút .btn-delete-goal ---
    goalListContainer.addEventListener('click', async function (event) {
        if (event.target.classList.contains('btn-delete-goal')) {
            const goalId = event.target.dataset.id;
            if (confirm('Bạn có chắc chắn muốn xóa mục tiêu này? Các thói quen liên quan có thể bị ảnh hưởng.')) {
                try {
                    const response = await fetch(`${API_GOALS_URL}/${goalId}`, {
                        method: 'DELETE',
                        // Thêm CSRF token header nếu cần
                    });

                    if (!response.ok) {
                        const errorData = await response.json().catch(() => ({ message: `Lỗi ${response.status}` }));
                        throw new Error(errorData.message || `HTTP error! status: ${response.status}`);
                    }
                    loadGoals();
                    alert('Mục tiêu đã được xóa!');
                } catch (error) {
                    console.error('Lỗi khi xóa mục tiêu:', error);
                    alert(`Không thể xóa mục tiêu: ${error.message}`);
                }
            }
        }
    });

    // --- 6. Xử lý sự kiện cho các nút .btn-update-progress ---
    goalListContainer.addEventListener('click', async function (event) {
        if (event.target.classList.contains('btn-update-progress')) {
            const goalId = event.target.dataset.id;
            const currentVal = parseFloat(event.target.dataset.current || 0);
            const targetVal = parseFloat(event.target.dataset.target);
            const unit = event.target.dataset.unit || '';

            // Đơn giản là dùng prompt, bạn có thể dùng modal đẹp hơn
            const newProgressStr = prompt(`Cập nhật tiến độ cho mục tiêu (Mục tiêu: ${targetVal} ${unit}):`, currentVal);

            if (newProgressStr !== null) { // Người dùng không nhấn Cancel
                const newProgress = parseFloat(newProgressStr);
                if (!isNaN(newProgress)) {
                    try {
                        const response = await fetch(`${API_GOALS_URL}/${goalId}/updateprogress`, {
                            method: 'PUT',
                            headers: {
                                'Content-Type': 'application/json',
                                // Thêm CSRF token header nếu cần
                            },
                            body: JSON.stringify({ currentValue: newProgress })
                        });
                        if (!response.ok) {
                            const errorData = await response.json().catch(() => ({ message: `Lỗi ${response.status}` }));
                            throw new Error(errorData.message || `HTTP error! status: ${response.status}`);
                        }
                        loadGoals();
                        alert('Tiến độ đã được cập nhật!');
                    } catch (error) {
                        console.error('Lỗi khi cập nhật tiến độ:', error);
                        alert(`Không thể cập nhật tiến độ: ${error.message}`);
                    }
                } else {
                    alert('Vui lòng nhập một số hợp lệ.');
                }
            }
        }
    });


    // --- Khởi tạo ---
    loadGoals(); // Tải mục tiêu khi trang được load

}); // Kết thúc DOMContentLoaded