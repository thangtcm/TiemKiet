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

	$('.select').each(function() {
		var $select = $(this);
		var modalId = $select.data('modal-id');
	
		if (modalId) {
			$select.select2({
				minimumResultsForSearch: -1,
				width: '100%',
				dropdownParent: $('#' + modalId)
			});
		} else {
			$select.select2({
				minimumResultsForSearch: -1,
				width: '100%'
			});
		}
	});
	

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

	// Datatable

	if ($('.datatable').length > 0) {
        $('.datatable').DataTable({
            "bFilter": false,
        });
    }
    if ($('.datatables').length > 0) {
        $('.datatables').DataTable({
            "bFilter": true,
        });
    }

	/*==================================================================
    [ +/- num product ]*/
    $('.btn-num-product-down').on('click', function(){
        var numProduct = Number($(this).next().val());
        if(numProduct > 0) $(this).next().val(numProduct - 1);
    });

    $('.btn-num-product-up').on('click', function(){
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


	/*------------------
		Navigation
	--------------------*/
    $(".mobile-menu").slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true,
		closedSymbol:"<i class='fas fa-caret-right'></i>",
    	openedSymbol:"<i class='fas fa-caret-down'></i>"
    });

	// //Canvas Menu
    // $(".canvas__open").on('click', function () {
    //     $(".offcanvas-menu-wrapper").addClass("active");
    //     $(".offcanvas-menu-overlay").addClass("active");
    // });

    // $(".offcanvas-menu-overlay").on('click', function () {
    //     $(".offcanvas-menu-wrapper").removeClass("active");
    //     $(".offcanvas-menu-overlay").removeClass("active");
    // });


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
		$(".owl-product").each(function() {
			// Cấu hình Owl Carousel
			$(this).owlCarousel({
				nav: true,
				dots: true,
				loop: true,
				center: true,
				margin: 20,
				responsive:{
					0:{
						items:1
					},
					600:{
						items:3
					},
					1000:{
						items:5
					}
				},
				loop: true,
				autoplay: true,
				autoplayHoverPause: true,
				navText: ["<i class=\"fas fa-chevron-left\"></i>", "<i class=\"fas fa-chevron-right\"></i>"],
			});
		});
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

	// $(document).ready(function () {
	// 	$(".otp-field").each(function () {
	// 		const inputs = $(this).find("input");
	// 		const button = $(this).closest(".modal-content").find(".btn-otp");

	// 		inputs.first().focus();
	// 		button.prop("disabled", true);

	// 		inputs.first().on("paste", function (event) {
	// 			event.preventDefault();

	// 			const pastedValue = (event.originalEvent.clipboardData || window.Clipboard).getData("text");

	// 			inputs.each(function (index) {
	// 				const currentInput = $(this);

	// 				if (index < pastedValue.length) {
	// 					currentInput.val(pastedValue[index]).removeAttr("disabled").focus();
	// 				} else {
	// 					currentInput.val("").focus();
	// 				}
	// 			});
	// 		});

	// 		inputs.each(function (index1) {
	// 			const currentInput = $(this);
	// 			const nextInput = currentInput.next("input");
	// 			const prevInput = currentInput.prev("input");

	// 			currentInput.on("input", function (e) {
	// 				if (nextInput.length && nextInput.prop("disabled") && currentInput.val() !== "") {
	// 					nextInput.removeAttr("disabled").focus();
	// 				}
	// 			});

	// 			currentInput.on("keyup", function (e) {
	// 				if (currentInput.val().length > 1) {
	// 					currentInput.val("");
	// 					return;
	// 				}

	// 				if (e.key === "Backspace" || e.key === "Delete") {
	// 					inputs.each(function (index2) {
	// 						const input = $(this);

	// 						if (index1 <= index2 && prevInput.length) {
	// 							input.prop("disabled", true).val("");
	// 							prevInput.focus();
	// 						}
	// 					});
	// 				}

	// 				button.removeClass("active").prop("disabled", true);

	// 				if (!inputs.last().prop("disabled") && inputs.last().val() !== "") {
	// 					button.addClass("active").prop("disabled", false);
	// 				}
	// 			});
	// 		});
	// 	});
	// });
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
				console.log(fieldIndex + '   ' + isValidInput +  '  ' + inputs.length+ '    ' + (fieldIndex == inputs.length - 1 && isValidInput) );
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
		$('#input_date').datetimepicker({
		useCurrent: false,
		allowInputToggle: true,
		showClose: true,
		showClear: true,
		showTodayButton: true,
		format: "DD/MM/YYYY hh:mm:ss A",
		icons: {
				time:'fas fa-clock',

				date:'fas fa-clock',

				up:'fa fa-chevron-up',

				down:'fa fa-chevron-down',

				previous:'fa fa-chevron-left',

				next:'fa fa-chevron-right',

				today:'fa fa-chevron-up',

				clear:'fa fa-trash',

				close:'fas fa-times'
			},
		debug:true
		});
	}

	if ($('#input_day').length > 0) {
		$('#input_day').datetimepicker({
		useCurrent: false,
		allowInputToggle: true,
		showClose: true,
		showClear: true,
		showTodayButton: true,
		format: "DD/MM/YYYY",
		icons: {
				time:'fas fa-clock',

				date:'fas fa-clock',

				up:'fa fa-chevron-up',

				down:'fa fa-chevron-down',

				previous:'fa fa-chevron-left',

				next:'fa fa-chevron-right',

				today:'fa fa-chevron-up',

				clear:'fa fa-trash',

				close:'fas fa-times'
			},
		debug:true
		});
	}
	$('#formregister').on('submit', function (event) {
		event.preventDefault();
	
	});

	if($("#imageUpload").length > 0)
	{
		$("#imageUpload").change(function(data){

		var imageFile = data.target.files[0];
		var reader = new FileReader();
		reader.readAsDataURL(imageFile);
	
		reader.onload = function(evt){
			$('#imagePreview').attr('src', evt.target.result);
			$('#imagePreview').hide();
			$('#imagePreview').fadeIn(650);
		}
		
		});
	}
	
})(jQuery);

function isValidPhoneNumber(phoneNumber) {
	var phoneRegex = /^(0[1-9][0-9]{8,9})$/;
	return phoneRegex.test(phoneNumber);
}

function sendOTP() {
	var phoneNumber = $('#phoneNumber').val();

	$.ajax({
		url: '/Account/SendOTP',
		method: 'POST',
		data: { phoneNumber: phoneNumber },
		success: function (result) {
			$('#loginModal').modal('hide');
			$('#otpModalToggle').modal('show');
		},
		error: function () {
			alert('Error sending OTP.' + message);
		}
	});
}

function getOtpData(targetId) {
	const targetField = $("#" + targetId);

	const otpValues = targetField.find("input").map(function () {
		return $(this).val();
	}).get();

	const otpCode = otpValues.join('');

	return otpCode;
}

function confirmOTP() {
	var phoneNumber = $('#phoneNumber').val();
	const otpCode = getOtpData("ConfirmOTPContainer");
	$.ajax({
		url: '/Account/ConfirmOTP',
		method: 'POST',
		data: { phoneNumber: phoneNumber, codeOTP: otpCode},
		success: function (result) {
			$('#otpModalToggle').modal('hide');
			$('#CodePinModalToggle').modal('show');
		},
		error: function () {
			alert('Error sending OTP.');
		}
	});
}

function loginOTP() {
	var phoneNumber = $('#phoneNumber').val();
	var otpCode = getOtpData("OtpPINContainer");
	var passwordError = $('#passwordError');
	$.ajax({
		url: '/Account/Login',
		method: 'POST',
		data: { phoneNumber: phoneNumber, password: otpCode},
		error: function () {
			passwordError.text('Số điện thoại không hợp lệ.');
		}
	});
}
