$(function() {
    // ...
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "50000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
      }
      toastr.success('The process has been saved.', 'Success');
      toastr.error('The process has been saved.', 'Error');
      toastr.info('The process has been saved.', 'Info');
      toastr.warning('The process has been saved.', 'Warning');
});