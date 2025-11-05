(function($) {

    // Mobile menu
    $('.header__mobile-menu-btn').click(function() {
        $('body').addClass('mobile-menu-opened');
        $('.header__mobile-menu').addClass('opened');
    });

    $('.header__mobile-menu, .header__mobile-menu-btn').click(function(e) {
        e.stopPropagation();
    });

    $(document).click(function() {
        $('body').removeClass('mobile-menu-opened');
        $('.header__mobile-menu').removeClass('opened');
    });

    // Main slider (home)
    $('#main_slider').owlCarousel({
        loop: true,
        autoplay: true,
        autoplaySpeed: 1000,
        dotsSpeed: 1000,
        margin: 0,
        nav: false,
        dot: true,
        items: 1,
    })

    // Lessons slider
    $('#lessons_slider').owlCarousel({
        loop: false,
        margin: 30,
        nav: true,
        navContainer: '#lessons_slider_nav',
        dot: false,
        responsive:{
            0:{
                items: 1
            },
            600:{
                items: 2
            },
            1000:{
                items: 3
            }
        }
    })

    // Download app slider
    $('#download_app_slider').owlCarousel({
        loop: true,
        margin: 30,
        touchDrag: false,
        mouseDrag: false,
        nav: true,
        center: true,
        navContainer: '#download_app_slider_nav',
        dot: false,
        responsive:{
            0:{
                items: 1
            },
            800:{
                items: 2
            },
            1000:{
                items: 3
            }
        }
    })

    // Lessons slider
    $('#presentations_slider').owlCarousel({
        loop: false,
        margin: 30,
        nav: true,
        navContainer: '#presentations_slider_nav',
        dot: false,
        responsive:{
            0:{
                items: 1
            },
            600:{
                items: 2
            },
            1000:{
                items: 3
            }
        }
    })

})(jQuery);