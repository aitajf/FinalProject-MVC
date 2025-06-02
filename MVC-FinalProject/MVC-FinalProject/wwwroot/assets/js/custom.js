"use strict"

//let button = document.querySelector(".show-more");
//let productsArea = document.querySelector(".products-area");
//let productCountInput = document.querySelector(".products-count");
//button.addEventListener("click", function () {
//    let skipProducts = productsArea.children.length;
//    let productCount = productCountInput.value;
//    console.log(productCount);
//    getData(skipProducts, productsArea, productCount, this);
//});

//async function getData(skip,elem, count, clickedElem) {
//    const url =`shop/showmore?skip=${skip}`;
//    try {
//        const response = await fetch(url);
//        const json = await response.text();
//        elem.innerHTML += json;
//        let updateSkip = document.querySelector(".products-area").children.length;
//        if (updateSkip == count) {
//            clickedElem.classList.add("d-none");
//        }

//    } catch (error) {
//        console.error(error.message);
//    }
//}


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


//$(function () {
//    $("#subscribeForm").on("submit", function (event) {
//        event.preventDefault();

//        var email = $("#emailInput").val();

//        $.ajax({
//            type: "POST",
//            url: "/Home/Subscribe",
//            data: JSON.stringify({ Email: email }),
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (response) {
//                console.log("Gələn JSON:", response);
//                $("#subscribeMessage").text(response.Message).show();
//            },
//            error: function () {
//                $("#subscribeMessage").text("Xəta baş verdi!").show();
//            }
//        });
//    });
//});
			