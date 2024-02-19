'use strict';
$(document).ready(function () {
    var isInstrunctionCollapse = false;
    $(".archives-header-polls").on('click', function () {
        $(".archives-polls").slideToggle();
    });

    $(".task-right-header-action").on('click', function() {
        $(".task-right-content-action").slideToggle();
    });

    $(".task-right-header-status").on('click', function() {
        $(".taskboard-right-progress").slideToggle();
    });

    $(".task-right-header-users").on('click', function() {
        $(".taskboard-right-users").slideToggle();
    });

    $(".task-right-header-revision").on('click', function() {
        $(".taskboard-right-revision").slideToggle();
    });

    $(".task-right-schedule-lock").on('click', function () {
        $(".taskboard-right-progress-sch-lock").slideToggle();
    });

    $(".color-scheme-header-polls").on('click', function () {
        $(".color-scheme-polls").slideToggle();
    });

    $(".font-header-polls").on('click', function () {
        $(".font-polls").slideToggle();
    });

    $(".title-header-polls").on('click', function () {
        $(".title-polls").slideToggle();
    });

    $(".howtorespond-header-polls").on('click', function () {
        $(".howtorespond-polls").slideToggle();
    });

    $(".background-header-polls").on('click', function () {
        $(".background-polls").slideToggle();
    });


    $(".bars-header-polls").on('click', function () {
        $(".bars-polls").slideToggle();
    });

    $(".axis-label-header-polls").on('click', function () {
        $(".axis-label-polls").slideToggle();
    });

    $(".instruction-view-header-polls").on('click', function () {

        if (!isInstrunctionCollapse) {
            $('#nochart').show();
            $('#chart').hide();
            $(".instruction-view-header-polls").val(0);
            isInstrunctionCollapse = true;
        } else {
            $('#nochart').hide();
            $('#chart').show();
            $(".instruction-view-header-polls").val(1);
            isInstrunctionCollapse = false;
        }

        $(".instruction-view-polls").slideToggle();
    });

    $(".total-result-header-polls").on('click', function () {
        $(".total-result-polls").slideToggle();
    });

    $(".legend-header-polls").on('click', function () {
        $(".legend-polls").slideToggle();
    });

    $('.collapse').on('shown.bs.collapse', function() {
        $(this).parent().find(".fa-angle-up").removeClass("fa-angle-up").addClass("fa-angle-down");
    }).on('hidden.bs.collapse', function() {
        $(this).parent().find(".fa-angle-down").removeClass("fa-angle-down").addClass("fa-angle-up");
    });

});
