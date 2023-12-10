(function ($) {
	"use strict";

	function saveToken(token) {
		$.ajax({
			url: '/Account/SaveToken',
			method: 'POST',
			contentType: 'application/json',
			data: { token: token },
			success: function (result) {
				console.log('Token saved to server:', result);
			},
			error: function (xhr, status, error) {
				console.error('Error saving token to server:', error);
			}
		});
	}


	window.isValidPhoneNumber = function isValidPhoneNumber(phoneNumber) {
		var phoneRegex = /^(0[1-9][0-9]{8,9})$/;
		return phoneRegex.test(phoneNumber);
	}

	// Function to show or hide preloader
	window.togglePreloader = function togglePreloader(show) {
		const preloader = $('#js-preloader');
		if (show) {
			preloader.removeClass('loaded');
		} else {
			preloader.addClass('loaded');
		}
	}

	// Ajax wrapper function with preloader handling
	function performAjaxRequest(url, method, data, successCallback, errorCallback) {
		togglePreloader(true);

		$.ajax({
			url: url,
			method: method,
			data: data,
			success: function (result) {
				togglePreloader(false);
				if (successCallback) {
					successCallback(result);
				}
			},
			error: function (xhr, status, error) {
				togglePreloader(false);
				if (errorCallback) {
					errorCallback(xhr, status, error);
				}
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

	window.initializeForm = function initializeForm(formId, submitFunction) {
		$(document).ready(function () {
			$(formId).on('submit', function (event) {
				event.preventDefault();
				submitFunction();
			});
		});
	}

	window.sendOTP = function sendOTP() {
		var phoneNumber = $('#phoneNumber').val();

		performAjaxRequest('/Account/SendOTP', 'POST', { phoneNumber: phoneNumber }, function (result) {
			$('#loginModal').modal('hide');
			$('#otpModalToggle').modal('show');
		}, function () {
			alert('Error sending OTP.');
		});
	}

	window.confirmOTP = function confirmOTP() {
		var phoneNumber = $('#phoneNumber').val();
		const otpCode = getOtpData("ConfirmOTPContainer");

		performAjaxRequest('/Account/ConfirmOTP', 'POST', { phoneNumber: phoneNumber, codeOTP: otpCode }, function (result) {
			console.log(result);
			if (result.success) {
				if (result.register) {
					$('#otpModalToggle').modal('hide');
					$('#NumberPhone').val(phoneNumber);
					$('#NumberPhoneHidden').val(phoneNumber);
					$('#registerModal').modal('show');
				}
				else {
					$('#otpModalToggle').modal('hide');
					$('#CodePinModalToggle').modal('show');
				}
			} 
		}, function () {
		});
	}

	window.loginOTP = async function loginOTP() {
		var phoneNumber = $('#phoneNumber').val();
		var otpCode = getOtpData("OtpPINContainer");
		var passwordError = $('#passwordError');
		var token = await getToken();
		
		console.log('phone' + phoneNumber + 'Pass : ' + otpCode);
		performAjaxRequest('/Account/Login', 'POST', { phoneNumber: phoneNumber, password: otpCode, token: token }, function (result) {
			if (result.success) {
				window.location.href = result.redirectUrl;
			} else {
				console.log('Lỗi' + result.error);
			}
		}, function () {
			console.log('Lỗi khi gửi Ajax request');
		});
	}

	window.register = function register() {
		const otpCode = getOtpData("OtpPINRegister");
		$('#Password').val(otpCode);
		performAjaxRequest('/Account/Register', 'POST', getFormData('#formregister'), function (result) {
			if (result.success) {
				console.log('Đăng ký thành công');
				$('#registerModal').modal('hide');
			} else {
				console.log('Đăng ký không thành công. Lỗi: ', result.error);
			}
		}, function () {
			console.log('Lỗi khi gửi Ajax request');
		});
	}

	function getFormData(formId) {
		const formDataArray = $(formId).serializeArray();

		const formDataObject = {};
		formDataArray.forEach(item => {
			formDataObject[item.name] = item.value;
		});

		return formDataObject;
	}

	window.getDistrictByProvince = function getDistrictByProvince(provinceId) {
		var districtSelects = document.querySelectorAll("#DistrictSelect");
		districtSelects.innerHTML = "";

		if (provinceId !== "") {
			$.ajax({
				url: "/District/GetDistrictByProvince",
				type: "GET",
				data: { provinceId: provinceId },
				success: function (data) {
					var s = '<option value="">Chọn Quận/Huyện</option>';
					for (var i = 0; i < data.length; i++) {
						s += '<option value="' + data[i].value + '">' + data[i].text + '</option>';
					}
					districtSelects.forEach(districtSelect => {
						districtSelect.innerHTML = s;
					});
				}
			});
		}
	}
	initializeForm('#formregister', register);
	initializeForm('#confirmOTP', confirmOTP);
	initializeForm('#formlogin', loginOTP);
	window.ImgUpload = function ImgUpload() {
	var imgWrap = "";
	var imgArray = [];

	$('#Multiple-Image').each(function () {
		$(this).on('change', function (e) {
			console.log("RUNN");
			imgWrap = $('#Multiple_ImgPreview');
			console.log(imgWrap);
			var maxLength = $(this).attr('data-max_length');

			var files = e.target.files;
			var filesArr = Array.prototype.slice.call(files);
			var iterator = 0;
			filesArr.forEach(function (f, index) {

				if (!f.type.match('image.*')) {
					return;
				}

				if (imgArray.length > maxLength) {
					return false
				} else {
					var len = 0;
					for (var i = 0; i < imgArray.length; i++) {
						if (imgArray[i] !== undefined) {
							len++;
						}
					}
					if (len > maxLength) {
						return false;
					} else {
						imgArray.push(f);

						var reader = new FileReader();
						reader.onload = function (e) {
							var html = "<div class='upload_img-box'><div style='background-image: url(" + e.target.result + ")' data-number='" + $(".upload_img-close").length + "' data-file='" + f.name + "' class='img-bg'><div class='upload_img-close'></div></div></div>";
							imgWrap.append(html);
							iterator++;
						}
						reader.readAsDataURL(f);
					}
				}
			});
		});
	});

	$('body').on('click', ".upload_img-close", function (e) {
		var file = $(this).parent().data("file");
		for (var i = 0; i < imgArray.length; i++) {
			if (imgArray[i].name === file) {
				imgArray.splice(i, 1);
				break;
			}
		}
		$(this).parent().parent().remove();
	});
	}

})(jQuery);
