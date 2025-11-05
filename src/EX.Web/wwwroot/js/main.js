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

})(jQuery);