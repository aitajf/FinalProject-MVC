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




//////Login

////$(document).ready(function () {
////    $('.login').submit(function (e) {
////        e.preventDefault();

////        var form = $(this);
////        var actionUrl = form.attr('action') || '@Url.Action("Login", "Account")';
////        var formData = form.serialize();

////        $.post(actionUrl, formData, function (response) {
////            if (response.success) {
////                window.location.href = response.redirectUrl;
////            } else {
////                var errorHtml = '<div class="alert alert-danger">';
////                response.errors.forEach(function (err) {
////                    errorHtml += '<p>' + err + '</p>';
////                });
////                errorHtml += '</div>';
////                $('#loginErrors').html(errorHtml);
////            }
////        });
////    });
////});

//document.querySelector("form.login").addEventListener("submit", async function (e) {
//    e.preventDefault();

//    const form = e.target;
//    const formData = new FormData(form);

//    const response = await fetch(form.action || window.location.href, {
//        method: 'POST',
//        headers: {
//            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
//        },
//        body: formData
//    });

//    const result = await response.json();

//    const errorContainer = document.getElementById("loginErrors");
//    errorContainer.innerHTML = "";

//    if (result.success) {
//        window.location.href = result.redirectUrl;
//    } else if (result.errors) {
//        result.errors.forEach(err => {
//            const p = document.createElement("p");
//            p.classList.add("text-danger");
//            p.innerText = err;
//            errorContainer.appendChild(p);
//        });
//    }
//});

////Login eye icon
//function togglePasswordVisibility() {
//    var passwordInput = document.querySelector('input[asp-for="Password"]');
//    var icon = document.getElementById("togglePasswordIcon");

//    if (passwordInput.type === "password") {
//        passwordInput.type = "text";
//        icon.classList.remove("fa-eye");
//        icon.classList.add("fa-eye-slash");
//    } else {
//        passwordInput.type = "password";
//        icon.classList.remove("fa-eye-slash");
//        icon.classList.add("fa-eye");
//    }
//}