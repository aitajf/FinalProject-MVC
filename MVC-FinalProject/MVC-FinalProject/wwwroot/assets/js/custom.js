"use strict"

//Show more button
document.addEventListener("DOMContentLoaded", function () {
    let button = document.querySelector(".show-more");
    let productsArea = document.querySelector(".products-area");
    let productCountInput = document.querySelector(".products-count");

    button.addEventListener("click", function () {
        let skipProducts = productsArea.children.length;
        let productCount = parseInt(productCountInput?.value ?? "0");
        console.log("Product Count:", productCount);
        getData(skipProducts, productsArea, productCount, this);
    });

    async function getData(skip, elem, count, clickedElem) {
        const url = `shop/showmore?skip=${skip}`;
        try {
            const response = await fetch(url);
            const json = await response.text();
            elem.innerHTML += json;

            let updateSkip = document.querySelector(".products-area").children.length;
            if (updateSkip >= count) {
                clickedElem.classList.add("d-none");
            }

        } catch (error) {
            console.error("Error fetching data:", error.message);
        }
    }
});

//Blog page UI pagination
document.addEventListener('DOMContentLoaded', function () {
	document.querySelector('#pagination-content').addEventListener('click', function (e) {
		if (e.target.matches('.page-numbers[data-page]')) {
			e.preventDefault();
			const page = e.target.getAttribute('data-page');

			fetch(`?page=${page}`, {
				headers: {
					'X-Requested-With': 'XMLHttpRequest'
				}
			})
			.then(response => response.text())
		    .then(html => {
		    	const parser = new DOMParser();
		    	const doc = parser.parseFromString(html, 'text/html');
		    	const newContent = doc.querySelector('#pagination-content');
		    	if (newContent) {
		    		document.querySelector('#pagination-content').innerHTML = newContent.innerHTML;
		    		window.history.pushState(null, '', `?page=${page}`);
		    	}
		    });
		}
	});
});


//Delete without refresh




			