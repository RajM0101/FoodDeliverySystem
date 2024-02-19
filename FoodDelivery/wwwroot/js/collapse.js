$(document).on('click', ".archives-header-polls", function () {
    $(".archives-polls").slideToggle();
    toggleCaret($(this));
});

$(document).on('click', ".task-right-header-action", function () {
    $(".task-right-content-action").slideToggle();
});

$(document).on('click', ".task-right-header-status", function () {
    $(".taskboard-right-progress").slideToggle();
});

$(document).on('click', ".task-right-header-users", function () {
    $(".taskboard-right-users").slideToggle();
});

$(document).on('click', ".task-right-header-revision", function () {
    $(".taskboard-right-revision").slideToggle();
});

$(document).on('click', ".task-right-schedule-lock", function () {
    $(".taskboard-right-progress-sch-lock").slideToggle();
});

$(document).on('click', ".color-scheme-header-polls", function () {
    $(".color-scheme-polls").slideToggle();
    toggleCaret($(this));
});

$(document).on('click', ".font-header-polls", function () {
    $(".font-polls").slideToggle();  
    toggleCaret($(this));
});

$(document).on('click', ".title-header-polls", function () {
    $(".title-polls").slideToggle();
    toggleCaret($(this));
});

$(document).on('click', ".howtorespond-header-polls", function () {
    $(".howtorespond-polls").slideToggle();
    toggleCaret($(this));
});

$(document).on('click', ".background-header-polls", function () {
    $(".background-polls").slideToggle();
    toggleCaret($(this));
});

$(document).on('click', ".bars-header-polls", function () {
    $(".bars-polls").slideToggle();
    toggleCaret($(this));
});

$(document).on('click', ".axis-label-header-polls", function () {
    $(".axis-label-polls").slideToggle();
    toggleCaret($(this));
});

$(document).on('click', ".instruction-view-header-polls", function () {
    var sPollId = $("#hdsPollId").val();
    if (!isInstrunctionCollapse) {
        $('#nochart_' + sPollId).show();
        $('#chart_' + sPollId).hide();
        $("#no_response_receive_" + sPollId).hide();

        $("#dvNochart").html('');
        $.each(modelChartData, function (index, gData) {
            if (noChartTmpl != undefined && noChartTmpl != "") {
                var htmlGroup = noChartTmpl.render(gData);
                $("#dvNochart").append(htmlGroup);
            }
        });
        isInstrunctionCollapse = true;
    } else {
        $('#nochart_' + sPollId).hide();
        $('#chart_' + sPollId).show();
        isInstrunctionCollapse = false;
    }

    $(".instruction-view-polls").slideToggle();
    toggleCaret($(this));
});

$(document).on('click', ".total-result-header-polls", function () {
    $(".total-result-polls").slideToggle();
});

$(document).on('click', ".legend-header-polls", function () {
    $(".legend-polls").slideToggle();
    toggleCaret($(this));
});

$(document).on('click', ".participants-header-polls", function () {
    $(".participants-polls").slideToggle();
});

$(document).on('click', ".help-header-polls", function () {
    $(".help-polls").slideToggle();
});

$('.collapse').on('shown.bs.collapse', function () {
    $(this).parent().find(".fa-angle-up").removeClass("fa-angle-up").addClass("fa-angle-down");
}).on('hidden.bs.collapse', function () {
    $(this).parent().find(".fa-angle-down").removeClass("fa-angle-down").addClass("fa-angle-up");
});

$(document).on('click', ".task-right-download-app", function () {
    $(".taskboard-right-download-app").slideToggle();
});

$(document).on('click', ".task-right-poll-instruction", function () {
    $(".taskboard-right-poll-instruction").slideToggle();
});

function toggleCaret($this) {
    if ($($this).find("span").attr("data-toggle") == "collapse") {
        $($this).find("span").attr("data-toggle", "collapsed");
    }
    else {
        $($this).find("span").attr("data-toggle", "collapse");
    }
}