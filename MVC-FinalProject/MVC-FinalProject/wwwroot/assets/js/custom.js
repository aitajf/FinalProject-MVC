"use strict"

//Admin Panel Dashboard saat
function updateDateTime() {
    const now = new Date();

    const weekday = now.toLocaleDateString('en-US', { weekday: 'long' });
    const date = now.toLocaleDateString('en-US', { month: 'long', day: 'numeric', year: 'numeric' });

    const time = now.toLocaleTimeString('en-US', { hour12: false });

    const hours = now.getHours();
    let greeting = "Welcome!";
    if (hours < 12) greeting = "Good morning";
    else if (hours < 18) greeting = "Good afternoon";
    else greeting = "Good evening";

    document.getElementById('weekday').textContent = weekday;
    document.getElementById('date').textContent = date;
    document.getElementById('clock').textContent = time;
    document.getElementById('greeting').textContent = greeting;
}
updateDateTime();
setInterval(updateDateTime, 1000);


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


////Delete without refresh



//BlogPost Edit üçün Deletİmg metodu


function deleteImage(imageId) {
    const blogPostId = document.getElementById("blog-container")?.dataset.routeId;

    fetch(`/Admin/BlogPost/DeleteImage?blogPostId=${blogPostId}&blogPostImageId=${imageId}`, {
        method: "DELETE"
    }).then(response => {
        if (!response.ok) {
            console.error("Error:", response.status, response.statusText);
            return;
        }

        const deletedImageDiv = document.getElementById(`image-${imageId}`);
        deletedImageDiv?.remove();

        const remainingImages = document.querySelectorAll('[id^="image-"]');

        if (remainingImages.length === 1) {
            const deleteBtn = remainingImages[0].querySelector("button");
            if (deleteBtn) {
                deleteBtn.style.display = "none";
            }
        }
    }).catch(error => {
        console.error("Error:", error);
    });
}



document.addEventListener("DOMContentLoaded", function () {
    const deleteButtons = document.querySelectorAll(".delete-btn");

    deleteButtons.forEach(button => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("data-id");

                fetch(`/Admin/Slider/Delete/${id}`, {
                    method: "POST",
                    headers: {
                        "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
                    }
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            const row = document.getElementById(`row-${id}`);
                            if (row) row.remove();
                        } else {
                            alert("Silinmə zamanı xəta baş verdi.");
                        }
                    });
        });
    });
});
