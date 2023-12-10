(function ($) {
	"use strict";
	// Install Animation 
	AOS.init();
	// Loading 
	$(window).on('load', function () {

		$('#js-preloader').addClass('loaded');

	});

	$(window).on('load', function () {
		if ($(".wow").length) {
			var wow = new wow({
				boxClass: 'wow',      // Animated element css class (default is wow)
				animateClass: 'animated', // Animation css class (default is animated)
				offset: 20,         // Distance to the element when triggering the animation (default is 0)
				mobile: true,       // Trigger animations on mobile devices (default is true)
				live: true,       // Act on asynchronously loaded content (default is true)
			});
			wow.init();
		}
	});

	// Select 2

	if ($('.selectsearch').length > 0) {
		$('.selectsearch').select2({
			minimumResultsForSearch: 1,
			width: '100%'
		});
	}

	if ($('.select').length > 0) {
		$('.select').select2({
			minimumResultsForSearch: -1,
			width: '100%'
		});
	}

	// Scroll To Top 
	$(window).on('scroll', function () {
		if ($(this).scrollTop() > 600) {
			$('.return-to-top').fadeIn();
		} else {
			$('.return-to-top').fadeOut();
		}
	});
	$('.return-to-top').on('click', function () {
		$('html, body').animate({
			scrollTop: 0
		}, 1500);
		return false;
	});

	/*==================================================================
	[ +/- num product ]*/
	$('.btn-num-product-down').on('click', function () {
		var numProduct = Number($(this).next().val());
		if (numProduct > 0) $(this).next().val(numProduct - 1);
	});

	$('.btn-num-product-up').on('click', function () {
		var numProduct = Number($(this).prev().val());
		$(this).prev().val(numProduct + 1);
	});

	$(window).stellar({
		responsive: true,
		parallaxBackgrounds: true,
		parallaxElements: true,
		horizontalScrolling: false,
		hideDistantElements: false,
		scrollProperty: 'scroll'
	});

	var fullHeight = function () {

		$('.js-fullheight').css('height', $(window).height());
		$(window).resize(function () {
			$('.js-fullheight').css('height', $(window).height());
		});

	};
	fullHeight();
	var siteSticky = function () {
		$(".js-sticky-header").sticky({ topSpacing: 0 });
	};
	siteSticky();

	/*------------------
		Navigation
	--------------------*/
	$(".mobile-menu").slicknav({
		prependTo: '#mobile-menu-wrap',
		allowParentLinks: true,
		closedSymbol: "<i class='fas fa-caret-right'></i>",
		openedSymbol: "<i class='fas fa-caret-down'></i>"
	});

	/**
	 * Hide mobile nav on same-page/hash links
	 */
	document.querySelectorAll('#navbar a').forEach(navbarlink => {

		if (!navbarlink.hash) return;

		let section = document.querySelector(navbarlink.hash);
		if (!section) return;

		navbarlink.addEventListener('click', () => {
			if (document.querySelector('.mobile-nav-active')) {
				mobileNavToogle();
			}
		});

	});

	/**
	 * Toggle mobile nav dropdowns
	 */
	const navDropdowns = document.querySelectorAll('.navbar .dropdown > a');

	navDropdowns.forEach(el => {
		el.addEventListener('click', function (event) {
			if (document.querySelector('.mobile-nav-active')) {
				event.preventDefault();
				this.classList.toggle('active');
				this.nextElementSibling.classList.toggle('dropdown-active');
			}
		})
	});

	var scrollWindow = function () {
		$(window).scroll(function () {
			var $w = $(this),
				st = $w.scrollTop(),
				navbar = $('.header'),
				sd = $('.js-scroll-wrap');

			if (st > 150) {
				if (!navbar.hasClass('scrolled')) {
					navbar.addClass('scrolled');
				}
			}
			if (st < 150) {
				if (navbar.hasClass('scrolled')) {
					navbar.removeClass('scrolled sleep');
				}
			}
			if (st > 350) {
				if (!navbar.hasClass('awake')) {
					navbar.addClass('awake');
				}

				if (sd.length > 0) {
					sd.addClass('sleep');
				}
			}
			if (st < 350) {
				if (navbar.hasClass('awake')) {
					navbar.removeClass('awake');
					navbar.addClass('sleep');
				}
				if (sd.length > 0) {
					sd.removeClass('sleep');
				}
			}
		});
	};
	scrollWindow();
	$("#owl-banner").owlCarousel({
		nav: true,
		dots: true,
		loop: true,
		center: true,
		margin: 20,
		items: 1,
		loop: true,
		autoplay: true,
		autoplayHoverPause: true,
		navText: ["<i class=\"fas fa-chevron-left\"></i>", "<i class=\"fas fa-chevron-right\"></i>"],
		autoplayTimeout: ("5" * 1000) || 7000,
	});

	$(document).ready(function() {
		$(".owl-branch").each(function() {
			// Cấu hình Owl Carousel
			$(this).owlCarousel({
				nav: true,
				dots: true,
				loop: true,
				center: true,
				margin: 20,
				items: 1,
				autoplay: true,
				autoplayHoverPause: true,
				navText: ["<i class=\"fas fa-chevron-left\"></i>", "<i class=\"fas fa-chevron-right\"></i>"],
				autoplayTimeout: ("5" * 1000) || 7000,
			});
		});
	});

	$(document).ready(function () {
		$(".owl-product").each(function () {
			// Cấu hình Owl Carousel
			$(this).owlCarousel({
				nav: true,
				dots: true,
				loop: true,
				center: true,
				margin: 20,
				responsive: {
					0: {
						items: 1
					},
					600: {
						items: 3
					},
					1000: {
						items: 5
					}
				},
				loop: true,
				autoplay: true,
				autoplayHoverPause: true,
				navText: ["<i class=\"fas fa-chevron-left\"></i>", "<i class=\"fas fa-chevron-right\"></i>"],
				autoplayTimeout: ("5" * 1000) || 7000,
			});
		});
	});
	
	//Input phone
	$(document).ready(function () {
		var inputPhone = $("input[type='tel']");

		if (inputPhone.length > 0) {
			inputPhone.each(function (index, element) {
				window.intlTelInput(element, {
					utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@18.2.1/build/js/utils.js",
					separateDialCode: true,
					onlyCountries: ["vn"],
					initialCountry: "vn"
				});
			});
		}
	});


	//Input OTP
	
	$(document).ready(function () {
		$(".otp-field").each(function () {
			const inputs = $(this).find("input");
			const buttons = $(this).closest(".modal-content").find(".btn-otp");
			buttons.prop("disabled", true);
			inputs.each(function (index) {
				$(this).data("index", index);
				$(this).on("keyup", handleOtp);
				$(this).on("paste", handleOnPasteOtp);
			});

			function handleOtp(e) {
				const input = $(this);
				let value = input.val();
				let isValidInput = value.match(/[0-9a-z]/gi);
				input.val("");
				input.val(isValidInput ? value[0] : "");

				let fieldIndex = input.data("index");
				if (fieldIndex < inputs.length - 1 && isValidInput) {
					input.next().focus();
				}

				if (e.key === "Backspace" && fieldIndex > 0) {
					input.prev().focus();
				}
				buttons.prop("disabled", true);
				console.log(fieldIndex + '   ' + isValidInput + '  ' + inputs.length + '    ' + (fieldIndex == inputs.length - 1 && isValidInput));
				if (fieldIndex == inputs.length - 1 && isValidInput) {
					buttons.prop("disabled", false);
				}
			}

			function handleOnPasteOtp(e) {
				const data = e.originalEvent.clipboardData.getData("text");
				const value = data.split("");
				if (value.length === inputs.length) {
					inputs.each(function (index) {
						$(this).val(value[index]);
						buttons.prop("disabled", false);
					});
				}
			}

			//   function submit() {
			// 	console.log("Submitting...");
			// 	let otp = "";
			// 	inputs.each(function () {
			// 	  otp += $(this).val();
			// 	  $(this).prop("disabled", true);
			// 	  $(this).addClass("disabled");
			// 	});
			// 	console.log(otp);
			// 	// Call API here
			//   }
		});
	});

	if ($('#input_date').length > 0) {
		var format = $('#input_date').data('format') || "DD/MM/YYYY hh:mm:ss A";
		$('#input_date').datetimepicker({
			allowInputToggle: true,
			showClose: true,
			showClear: true,
			showTodayButton: true,
			format: format,
			icons: {
				time: 'fas fa-clock',

				date: 'fas fa-clock',

				up: 'fa fa-chevron-up',

				down: 'fa fa-chevron-down',

				previous: 'fa fa-chevron-left',

				next: 'fa fa-chevron-right',

				today: 'fa fa-chevron-up',

				clear: 'fa fa-trash',

				close: 'fas fa-times'
			},
		});
	}

	if ($("#imageUpload").length > 0) {
		$("#imageUpload").change(function (data) {

			var imageFile = data.target.files[0];
			var reader = new FileReader();
			reader.readAsDataURL(imageFile);

			reader.onload = function (evt) {
				$('#imagePreview').attr('src', evt.target.result);
				$('#imagePreview').hide();
				$('#imagePreview').fadeIn(650);
			}

		});
	}
})(jQuery);
