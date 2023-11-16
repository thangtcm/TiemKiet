(function ($) {
    "use strict";
	// Install Animation 
    AOS.init();
	// Loading 
	$(window).on('load', function() {

        $('#js-preloader').addClass('loaded');

    });

	$(window).on ('load', function (){
        if ($(".wow").length) { 
            var wow = new wow({
                boxClass:     'wow',      // Animated element css class (default is wow)
                animateClass: 'animated', // Animation css class (default is animated)
                offset:       20,         // Distance to the element when triggering the animation (default is 0)
                mobile:       true,       // Trigger animations on mobile devices (default is true)
                live:         true,       // Act on asynchronously loaded content (default is true)
            });
            wow.init();
        }
    });

	// Select 2
	
	if ($('.select').length > 0) {
		$('.select').select2({
			minimumResultsForSearch: 1,
			width: '100%'
		});
	}

	// Scroll To Top 
	$(window).on('scroll',function () {
		if ($(this).scrollTop() > 600) {
			$('.return-to-top').fadeIn();
		} else {
			$('.return-to-top').fadeOut();
		}
	});
	$('.return-to-top').on('click',function(){
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
    var siteSticky = function() {
		$(".js-sticky-header").sticky({topSpacing:0});
	};
	siteSticky();

	var siteMenuClone = function() {

		$('.js-clone-nav').each(function() {
			var $this = $(this);
			$this.clone().attr('class', 'site-nav-wrap').appendTo('.site-mobile-menu-body');
		});


		setTimeout(function() {
			
			var counter = 0;
      $('.site-mobile-menu .has-children').each(function(){
        var $this = $(this);
        
        $this.prepend('<span class="arrow-collapse collapsed">');

        $this.find('.arrow-collapse').attr({
          'data-toggle' : 'collapse',
          'data-target' : '#collapseItem' + counter,
        });

        $this.find('> ul').attr({
          'class' : 'collapse',
          'id' : 'collapseItem' + counter,
        });

        counter++;

      });

    }, 1000);

		$('body').on('click', '.arrow-collapse', function(e) {
      var $this = $(this);
      if ( $this.closest('li').find('.collapse').hasClass('show') ) {
        $this.removeClass('active');
      } else {
        $this.addClass('active');
      }
      e.preventDefault();  
      
    });

		$(window).resize(function() {
			var $this = $(this),
				w = $this.width();

			if ( w > 768 ) {
				if ( $('body').hasClass('offcanvas-menu') ) {
					$('body').removeClass('offcanvas-menu');
				}
			}
		})

		$('body').on('click', '.js-menu-toggle', function(e) {
			var $this = $(this);
			e.preventDefault();

			if ( $('body').hasClass('offcanvas-menu') ) {
				$('body').removeClass('offcanvas-menu');
				$this.removeClass('active');
			} else {
				$('body').addClass('offcanvas-menu');
				$this.addClass('active');
			}
		}) 

		// click outisde offcanvas
		$(document).mouseup(function(e) {
	    var container = $(".site-mobile-menu");
	    if (!container.is(e.target) && container.has(e.target).length === 0) {
	      if ( $('body').hasClass('offcanvas-menu') ) {
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
	  el.addEventListener('click', function(event) {
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
	  el.addEventListener('click', function(event) {
		if (document.querySelector('.mobile-nav-active')) {
		  event.preventDefault();
		  this.classList.toggle('active');
		  this.nextElementSibling.classList.toggle('dropdown-active');
		}
	  })
	});

	

	var scrollWindow = function() {
		$(window).scroll(function(){
			var $w = $(this),
					st = $w.scrollTop(),
					navbar = $('.header'),
					sd = $('.js-scroll-wrap');

			if (st > 150) {
				if ( !navbar.hasClass('scrolled') ) {
					navbar.addClass('scrolled');	
				}
			} 
			if (st < 150) {
				if ( navbar.hasClass('scrolled') ) {
					navbar.removeClass('scrolled sleep');
				}
			} 
			if ( st > 350 ) {
				if ( !navbar.hasClass('awake') ) {
					navbar.addClass('awake');	
				}
				
				if(sd.length > 0) {
					sd.addClass('sleep');
				}
			}
			if ( st < 350 ) {
				if ( navbar.hasClass('awake') ) {
					navbar.removeClass('awake');
					navbar.addClass('sleep');
				}
				if(sd.length > 0) {
					sd.removeClass('sleep');
				}
			}
		});
	};
	scrollWindow();
	$("#owl-banner").owlCarousel({
		nav: true,
		dots: true,
		loop:true,
		center:true,
		margin: 20,
		items: 1,
		loop: true,
		autoplay: true,
		autoplayHoverPause:true,
		navText: ["<i class=\"fas fa-chevron-left\"></i>","<i class=\"fas fa-chevron-right\"></i>"],
		autoplayTimeout: ("5" * 1000) || 7000,
	});

	

})(jQuery);

