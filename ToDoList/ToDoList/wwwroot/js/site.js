function populateEditForm(id, title, description, isCompleted, dueDate, priority) {
    document.getElementById('editTaskId').value = id;
    document.getElementById('editTitle').value = title;
    document.getElementById('editDescription').value = description;
    document.getElementById('editStatus').value = isCompleted ? 'true' : 'false';;
    document.getElementById('editDueDate').value = dueDate;
    document.getElementById('editPriority').value = priority;
}

function populateDeleteForm(id) {
    document.getElementById('deleteTaskId').value = id;
}

document.addEventListener('DOMContentLoaded', function () {
    const urlParams = new URLSearchParams(window.location.search);

    const statusFilter = document.getElementById('statusFilter');
    const priorityFilter = document.getElementById('priorityFilter');

    if (urlParams.has('status')) {
        statusFilter.value = urlParams.get('status');
    }

    if (urlParams.has('priority')) {
        priorityFilter.value = urlParams.get('priority');
    }
});