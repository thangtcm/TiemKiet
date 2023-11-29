
function isValidPhoneNumber(phoneNumber) {
	var phoneRegex = /^(0[1-9][0-9]{8,9})$/;
	return phoneRegex.test(phoneNumber);
}

// Function to show or hide preloader
function togglePreloader(show) {
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

function sendOTP() {
	var phoneNumber = $('#phoneNumber').val();

	performAjaxRequest('/Account/SendOTP', 'POST', { phoneNumber: phoneNumber }, function (result) {
		$('#loginModal').modal('hide');
		$('#otpModalToggle').modal('show');
	}, function () {
		alert('Error sending OTP.');
	});
}

function confirmOTP() {
	var phoneNumber = $('#phoneNumber').val();
	const otpCode = getOtpData("ConfirmOTPContainer");

	performAjaxRequest('/Account/ConfirmOTP', 'POST', { phoneNumber: phoneNumber, codeOTP: otpCode }, function (result) {
		console.log(result);
		if (result.success) {
			if (result.register) {
				$('#otpModalToggle').modal('hide');
				$('#NumberPhone').val(phoneNumber);
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

function loginOTP() {
	var phoneNumber = $('#phoneNumber').val();
	var otpCode = getOtpData("OtpPINContainer");
	var passwordError = $('#passwordError');
	console.log('phone' + phoneNumber + 'Pass : ' + otpCode);
	performAjaxRequest('/Account/Login', 'POST', { phoneNumber: phoneNumber, password: otpCode }, function (result) {
		if (result.success) {
			window.location.href = result.redirectUrl;
		} else {
			console.log('Lỗi' + result.error);
		}
	}, function () {
		console.log('Lỗi khi gửi Ajax request');
	});
}

function register() {
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

function initializeForm(formId, submitFunction) {
	$(document).ready(function () {
		$(formId).on('submit', function (event) {
			event.preventDefault();
			submitFunction();
		});
	});
}

