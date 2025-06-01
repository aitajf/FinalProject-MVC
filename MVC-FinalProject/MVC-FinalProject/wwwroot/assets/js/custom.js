//"use strict"

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