﻿const activePage = window.location.pathname;
console.log(activePage);
const navLinks = document.querySelectorAll('div a').
    forEach(link => {
        if (link.href.includes(`${activePage}`)) {
            link.classList.add('active');
        }
    })
