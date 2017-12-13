$(document).ready(function () {

	var theForm = $("#the-form");
	theForm.hide();

	var button = $("#buy-button");

	button.on("click", function () {
		console.log('buying item');
	});

	var productInfo = $(".product-props li");

	productInfo.on("click", function () {
		console.log("You clicked on " + $(this).text());
	});

	var $loginToggle = $("#login-toggle");
	var $popupForm = $(".popup-form");

	$loginToggle.on("click", function () {
		$popupForm.slideToggle(1000);
	});

});


