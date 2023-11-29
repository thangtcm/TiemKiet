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

	var siteMenuClone = function () {

		$('.js-clone-nav').each(function () {
			var $this = $(this);
			$this.clone().attr('class', 'site-nav-wrap').appendTo('.site-mobile-menu-body');
		});


		setTimeout(function () {

			var counter = 0;
			$('.site-mobile-menu .has-children').each(function () {
				var $this = $(this);

				$this.prepend('<span class="arrow-collapse collapsed">');

				$this.find('.arrow-collapse').attr({
					'data-toggle': 'collapse',
					'data-target': '#collapseItem' + counter,
				});

				$this.find('> ul').attr({
					'class': 'collapse',
					'id': 'collapseItem' + counter,
				});

				counter++;

			});

		}, 1000);

		$('body').on('click', '.arrow-collapse', function (e) {
			var $this = $(this);
			if ($this.closest('li').find('.collapse').hasClass('show')) {
				$this.removeClass('active');
			} else {
				$this.addClass('active');
			}
			e.preventDefault();

		});

		$(window).resize(function () {
			var $this = $(this),
				w = $this.width();

			if (w > 768) {
				if ($('body').hasClass('offcanvas-menu')) {
					$('body').removeClass('offcanvas-menu');
				}
			}
		})

		$('body').on('click', '.js-menu-toggle', function (e) {
			var $this = $(this);
			e.preventDefault();

			if ($('body').hasClass('offcanvas-menu')) {
				$('body').removeClass('offcanvas-menu');
				$this.removeClass('active');
			} else {
				$('body').addClass('offcanvas-menu');
				$this.addClass('active');
			}
		})

		// click outisde offcanvas
		$(document).mouseup(function (e) {
			var container = $(".site-mobile-menu");
			if (!container.is(e.target) && container.has(e.target).length === 0) {
				if ($('body').hasClass('offcanvas-menu')) {
					$('body').removeClass('offcanvas-menu');
				}
			}
		});
	};
	siteMenuClone();

	/**
   * Mobile nav toggle
   */

	const mobileNavShow = document.querySelector('.mobile-nav-show');
	const mobileNavHide = document.querySelector('.mobile-nav-hide');

	document.querySelectorAll('.mobile-nav-toggle').forEach(el => {
		el.addEventListener('click', function (event) {
			event.preventDefault();
			mobileNavToogle();
		})
	});

	function mobileNavToogle() {
		document.querySelector('body').classList.toggle('mobile-nav-active');
		mobileNavShow.classList.toggle('d-none');
		mobileNavHide.classList.toggle('d-none');
	}

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
	// $(document).ready(function () {
	// 	const inputs = $(".otp-field > input");
	// 	const button = $(".btn-otp");

	// 	inputs.first().focus();
	// 	button.prop("disabled", true);

	// 	inputs.first().on("paste", function (event) {
	// 	  event.preventDefault();

	// 	  const pastedValue = (event.originalEvent.clipboardData || window.Clipboard).getData("text");

	// 	  inputs.each(function (index) {
	// 		const currentInput = $(this);

	// 		if (index < pastedValue.length) {
	// 		  currentInput.val(pastedValue[index]).removeAttr("disabled").focus();
	// 		} else {
	// 		  currentInput.val("").focus();
	// 		}
	// 	  });
	// 	});

	// 	inputs.each(function (index1) {
	// 		const currentInput = $(this);
	// 		const nextInput = currentInput.next("input");
	// 		const prevInput = currentInput.prev("input");
	// 		currentInput.on("input", function (e){
	// 			if (nextInput.length && nextInput.prop("disabled") && currentInput.val() !== "") {
	// 				nextInput.removeAttr("disabled").focus();
	// 			}
	// 		})
	// 		currentInput.on("keyup", function (e) {
	// 			if (currentInput.val().length > 1) {
	// 				currentInput.val("");
	// 				return;
	// 			}

	// 			if (e.key === "Backspace" || e.key == "delete") {
	// 				inputs.each(function (index2) {
	// 					const input = $(this);

	// 					if (index1 <= index2 && prevInput.length) {
	// 					input.prop("disabled", true).val("");
	// 					prevInput.focus();
	// 					}
	// 				});
	// 			}

	// 			button.removeClass("active").prop("disabled", true);

	// 			if (!inputs.last().prop("disabled") && inputs.last().val() !== "") {
	// 				button.addClass("active").prop("disabled", false);
	// 			}
	// 	  	});
	// 	});
	// });

	$(document).ready(function () {
		$(".otp-field").each(function () {
			const inputs = $(this).find("input");
			const button = $(this).closest(".modal-content").find(".btn-otp");

			inputs.first().focus();
			button.prop("disabled", true);

			inputs.first().on("paste", function (event) {
				event.preventDefault();

				const pastedValue = (event.originalEvent.clipboardData || window.Clipboard).getData("text");

				inputs.each(function (index) {
					const currentInput = $(this);

					if (index < pastedValue.length) {
						currentInput.val(pastedValue[index]).removeAttr("disabled").focus();
					} else {
						currentInput.val("").focus();
					}
				});
			});

			inputs.each(function (index1) {
				const currentInput = $(this);
				const nextInput = currentInput.next("input");
				const prevInput = currentInput.prev("input");

				currentInput.on("input", function (e) {
					if (nextInput.length && nextInput.prop("disabled") && currentInput.val() !== "") {
						nextInput.removeAttr("disabled").focus();
					}
				});

				currentInput.on("keyup", function (e) {
					if (currentInput.val().length > 1) {
						currentInput.val("");
						return;
					}

					if (e.key === "Backspace" || e.key === "Delete") {
						inputs.each(function (index2) {
							const input = $(this);

							if (index1 <= index2 && prevInput.length) {
								input.prop("disabled", true).val("");
								prevInput.focus();
							}
						});
					}

					button.removeClass("active").prop("disabled", true);

					if (!inputs.last().prop("disabled") && inputs.last().val() !== "") {
						button.addClass("active").prop("disabled", false);
					}
				});
			});
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
})(jQuery);
