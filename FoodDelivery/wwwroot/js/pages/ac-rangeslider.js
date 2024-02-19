$(window).on('load', function () {

    $(function () {
        var slider = new Slider('#title', {
            formatter: function (value) {
                return 'Current Size: ' + value;
            }
        });
    });

    $(function () {
        var slider = new Slider('#response', {
            formatter: function (value) {
                return 'Current Size: ' + value;
            }
        });
    });

});
