function getAntiForgeryTokenValue() {
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    return tokenInput ? tokenInput.value : null;
}

document.addEventListener('DOMContentLoaded', function () {
    const tabs = document.querySelectorAll('.tab');
    const tasks = document.querySelectorAll('.task-card');

    // ======= Tabs Filter =======
    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            tabs.forEach(t => t.classList.remove('active'));
            tab.classList.add('active');

            const filter = tab.dataset.filter;
            tasks.forEach(task => {
                if (filter === 'all') {
                    task.style.display = 'flex';
                } else if (filter === 'active') {
                    task.style.display = task.classList.contains('active') ? 'flex' : 'none';
                } else if (filter === 'completed') {
                    task.style.display = task.classList.contains('completed') ? 'flex' : 'none';
                }
            });
        });
    });

    // ======= Toggle Task Status =======
    document.querySelectorAll('[data-action="toggle"]').forEach(el => {
        el.addEventListener('click', async function () {
            const card = this.closest('.task-card');
            const taskId = parseInt(card.dataset.id, 10);
            const currentlyDone = this.classList.contains('done');
            const newIsDone = !currentlyDone;

            const token = getAntiForgeryTokenValue();
            if (!token) {
                alert('CSRF token not found. Refresh the page.');
                return;
            }

            try {
                const res = await fetch('/Tasks/ToggleTaskStatus', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': token
                    },
                    body: JSON.stringify({ id: taskId, isDone: newIsDone })
                });

                if (!res.ok) throw new Error(await res.text());
                const json = await res.json();

                if (json.success) {
                    const title = card.querySelector('.task-title');
                    if (json.isDone) {
                        this.classList.remove('not-done');
                        this.classList.add('done');
                        this.textContent = '✔';
                        title.classList.add('done');
                        card.classList.remove('active');
                        card.classList.add('completed');
                    } else {
                        this.classList.remove('done');
                        this.classList.add('not-done');
                        this.textContent = '';
                        title.classList.remove('done');
                        card.classList.remove('completed');
                        card.classList.add('active');
                    }
                }
            } catch (err) {
                console.error(err);
                alert('Connection error.');
            }
        });
    });
});
