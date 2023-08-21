document.addEventListener("DOMContentLoaded", function () {
    const modalCards = document.querySelectorAll('.card');
    const searchInput = document.getElementById('search');

    modalCards.forEach((card) => {
        const modalID = `userModal-${card.getAttribute('data-user-id')}`;
        const modal = document.getElementById(modalID);

        // Code to display modal on card click
        card.addEventListener('click', () => {
            modal.style.display = 'block';
        });

        // Store the searchable data in a data attribute of the card
        const parcelID = card.getAttribute('data-user-id');
        const senderName = card.querySelector('.head').textContent;
        const receiverName = card.querySelector('.body p:nth-child(4)').textContent;
        const deliveryMan = card.querySelector('.body p:nth-child(8)').textContent;
        const deliveryAddress = card.querySelector('.body p:nth-child(6)').textContent;
        const status = card.querySelector('.body p:nth-child(2)').textContent;
        card.setAttribute('data-searchable', `${parcelID} ${senderName} ${receiverName} ${deliveryMan} ${deliveryAddress} ${status}`);
    });

    searchInput.addEventListener('input', function () {
        const searchTerm = searchInput.value.trim().toLowerCase();
        modalCards.forEach((card) => {
            const searchableData = card.getAttribute('data-searchable').toLowerCase();
            if (searchableData.includes(searchTerm)) {
                card.style.display = 'block';
            } else {
                card.style.display = 'none';
            }
        });
    });

    document.addEventListener('click', (event) => {
        const closeIcon = event.target.closest('.close-modal');
        if (closeIcon) {
            const modal = closeIcon.closest('.modal');
            modal.style.display = 'none';
        }
    });
});
