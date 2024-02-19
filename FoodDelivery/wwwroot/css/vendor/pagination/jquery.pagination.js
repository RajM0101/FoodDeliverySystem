(function ($) {
    $.parser = {
        parseOptions: function ($this, $opt) {
            var t = $($this);
            var result = {};
            var $data = $.trim(t.attr("data-options"));
            if ($data) {
                var firstCh = $data.substring(0, 1);
                var lastCh = $data.substring($data.length - 1, 1);
                if (firstCh != "{") {
                    $data = "{" + $data;
                }
                if (lastCh != "}") {
                    $data = $data + "}";
                }
                result = (new Function("return " + $data))();
            }
            if ($opt) {
                var result1 = {};
                for (var i = 0; i < $opt.length; i++) {
                    var pp = $opt[i];
                    if (typeof pp == "string") {
                        if (pp == "width" || pp == "height" || pp == "left" || pp == "top") {
                            result1[pp] = parseInt($this.style[pp]) || undefined;
                        } else {
                            result1[pp] = t.attr(pp);
                        }
                    } else {
                        for (var item in pp) {
                            var _d = pp[item];
                            if (_d == "boolean") {
                                result1[item] = t.attr(item) ? (t.attr(item) == "true") : undefined;
                            } else {
                                if (_d == "number") {
                                    result1[item] = t.attr(item) == "0" ? 0 : parseFloat(t.attr(item)) || undefined;
                                }
                            }
                        }
                    }
                }
                $.extend(result, result1);
            }
            return result;
        }
    };
    function prepareLnkButton($context) {
        var $opt = $.data($context, "lnkbutton").options;
        $($context).empty();
        $($context).addClass("page-link l-btn");
        if ($opt.id) {
            $($context).attr("id", $opt.id);
        } else {
            $($context).attr("id", "");
        }
        if ($opt.plain) {
            $($context).addClass("l-btn-plain");
        } else {
            $($context).removeClass("l-btn-plain");
        }
        if ($opt.text) {
            $($context).html($opt.text).wrapInner("<span class=\"l-btn-left\">" + "<span class=\"l-btn-text\">" + "</span>" + "</span>");
            if ($opt.iconCls) {
                $($context).find(".l-btn-text").addClass($opt.iconCls).addClass($opt.iconAlign == "left" ? "l-btn-icon-left" : "l-btn-icon-right");
            }
        }
        else {
            if ($opt.iconCls == "pagination-first") {
                $($context).html('<i class="fa fa-angle-double-left icon"></i>');
            }
            else if ($opt.iconCls == "pagination-prev") {
                $($context).html('<i class="fa fa-angle-left icon"></i>');
            }
            else if ($opt.iconCls == "pagination-next") {
                $($context).html('<i class="fa fa-angle-right icon"></i>');
            }
            else if ($opt.iconCls == "pagination-last") {
                $($context).html('<i class="fa fa-angle-double-right icon"></i>');
            }
            else if ($opt.iconCls == "pagination-refresh" || $opt.iconCls == "pagination-load") {
                $($context).html('<i class="fa fa-refresh icon icon-small"></i>');
            }
            else {
                $($context).html("&nbsp;").wrapInner("<span class=\"l-btn-left\">" + "<span class=\"l-btn-text\">" + "<span class=\"l-btn-empty\"></span>" + "</span>" + "</span>");
                if ($opt.iconCls) {
                    $($context).find(".l-btn-empty").addClass($opt.iconCls);
                }
            }
        }
        $($context).unbind(".lnkbutton").bind("focus.lnkbutton", function () {
            if (!$opt.disabled) {
                $(this).find("span.l-btn-text").addClass("l-btn-focus");
            }
        }).bind("blur.lnkbutton", function () {
            $(this).find("span.l-btn-text").removeClass("l-btn-focus");
        });
        updateState($context, $opt.disabled);
    };
    function updateState($this, IsEnable) {
        var $setting = $.data($this, "lnkbutton");

        if (IsEnable) {
            $setting.options.disabled = true;
            var $href = $($this).attr("href");
            if ($href) {
                $setting.href = $href;
                $($this).attr("href", "javascript:void(0)");
            }
            if ($this.onclick) {
                $setting.onclick = $this.onclick;
                $this.onclick = null;
            }
            $($this).addClass("l-btn-disabled");
        } else {
            $setting.options.disabled = false;
            if ($setting.href) {
                $($this).attr("href", $setting.href);
            }
            if ($setting.onclick) {
                $this.onclick = $setting.onclick;
            }
            $($this).removeClass("l-btn-disabled");
        }
    };
    $.fn.lnkbutton = function (customOption, $button) {
        if (typeof customOption == "string") {
            return $.fn.lnkbutton.methods[customOption](this, $button);
        }
        customOption = customOption || {};
        return this.each(function () {
            var $this = $.data(this, "lnkbutton");
            if ($this) {
                $.extend($this.options, customOption);
            } else {
                $.data(this, "lnkbutton", {
                    options: $.extend({}, $.fn.lnkbutton.defaults, $.fn.lnkbutton.parseOptions(this), customOption)
                });
                $(this).removeAttr("disabled");
            }
            prepareLnkButton(this);
        });
    };
    $.fn.lnkbutton.methods = {
        options: function ($this) {
            return $.data($this[0], "lnkbutton").options;
        },
        enable: function ($this) {
            return $this.each(function () {
                updateState(this, false);
            });
        },
        disable: function ($this) {
            return $this.each(function () {
                updateState(this, true);
            });
        }
    };
    $.fn.lnkbutton.parseOptions = function ($this) {
        var t = $($this);
        return $.extend({}, $.parser.parseOptions($this, ["id", "iconCls", "iconAlign", {
            plain: "boolean"
        }]), {
            disabled: (t.attr("disabled") ? true : undefined),
            text: $.trim(t.html()),
            iconCls: (t.attr("icon") || t.attr("iconCls"))
        });
    };
    $.fn.lnkbutton.defaults = {
        id: null,
        disabled: false,
        plain: false,
        text: "",
        iconCls: null,
        iconAlign: "left"
    };

    function preparePaging($context) {
        var pagination = $.data($context, "pagination");
        var $opt = pagination.options;
        var pagingItems = pagination.pagingItems = {};
        var pagingTable = $($context).addClass("pagination").html("<table class=\"pagination\" cellspacing=\"0\" cellpadding=\"2\" border=\"0\"><tr></tr></table>");
        var tr = pagingTable.find("tr");

        function preparePageItem(pageItemName) {
            var pageItem = $opt.nav[pageItemName];
            var a = $("<a href=\"javascript:void(0)\"></a>").appendTo(tr);
            a.wrap("<td></td>");
            a.lnkbutton({
                iconCls: pageItem.iconCls,
                plain: true
            }).unbind(".pagination").bind("click.pagination", function () {
                pageItem.handler.call($context);
            });
            return a;
        };
        if ($opt.showPageList) {
            var ps = $("<select class=\"pagination-page-list\"></select>");
            ps.bind("change", function (ev) { 
                var $IsvalidToChange = $opt.onValidate.call(this);
                if ($IsvalidToChange) {
                    $opt.pageSize = parseInt($(this).val());
                    $opt.onChangePageSize.call($context, $opt.pageSize);
                    changePage($context, $opt.pageNumber);
                }
                else {
                    ps.val($opt.pageSize); 
                }
            });
            for (var i = 0; i < $opt.pageList.length; i++) {
                $("<option></option>").text($opt.pageList[i]).appendTo(ps);
            }
            $("<td></td>").append(ps).appendTo(tr);
            $("<td><div class=\"pagination-btn-separator\"></div></td>").appendTo(tr);
        }
        pagingItems.first = preparePageItem("first");
        pagingItems.prev = preparePageItem("prev");
        $("<td><div class=\"pagination-btn-separator\"></div></td>").appendTo(tr);
        $("<span style=\"padding-left:6px;\"></span>").html($opt.beforePageText).appendTo(tr).wrap("<td></td>");
        pagingItems.num = $("<input class=\"form-control\" type=\"text\" value=\"1\" size=\"2\">").appendTo(tr).wrap("<td></td>");
        pagingItems.num.unbind(".pagination").bind("keydown.pagination", function (e) {
            if (e.keyCode == 13) {
                var $IsvalidToChange = $opt.onValidate.call(this);
                if ($IsvalidToChange) {
                    var pageNumber = parseInt($(this).val()) || 1;
                    changePage($context, pageNumber);
                    return false;
                }                
            }
        });
        pagingItems.after = $("<span style=\"padding-right:6px;\"></span>").appendTo(tr).wrap("<td></td>");
        $("<td><div class=\"pagination-btn-separator\"></div></td>").appendTo(tr);
        pagingItems.next = preparePageItem("next");
        pagingItems.last = preparePageItem("last");
        if ($opt.showRefresh) {
            $("<td><div class=\"pagination-btn-separator\"></div></td>").appendTo(tr);
            pagingItems.refresh = preparePageItem("refresh");
        }
        if ($opt.buttons) {
            $("<td><div class=\"pagination-btn-separator\"></div></td>").appendTo(tr);
            for (var i = 0; i < $opt.buttons.length; i++) {
                var button = $opt.buttons[i];
                if (button == "-") {
                    $("<td><div class=\"pagination-btn-separator\"></div></td>").appendTo(tr);
                } else {
                    var td = $("<td></td>").appendTo(tr);
                    $("<a href=\"javascript:void(0)\"></a>").appendTo(td).lnkbutton($.extend(button, {
                        plain: true
                    })).bind("click", eval(button.handler || function () { }));
                }
            }
        }
        $("<div class=\"pagination-info\"></div>").appendTo(pagingTable);
        $("<div style=\"clear:both;\"></div>").appendTo(pagingTable);
    };
    function changePage($context, pageNumber) {
        var $opt = $.data($context, "pagination").options;
        var totalpages = Math.ceil($opt.total / $opt.pageSize) || 1;
        $opt.pageNumber = pageNumber;
        if ($opt.pageNumber < 1) {
            $opt.pageNumber = 1;
        }
        if ($opt.pageNumber > totalpages) {
            $opt.pageNumber = totalpages;
        }
        prepareDisplayMsg($context, {
            pageNumber: $opt.pageNumber
        });
        $opt.onSelectPage.call($context, $opt.pageNumber, $opt.pageSize);
    };
    function prepareDisplayMsg($context, customOption) {
        var $opt = $.data($context, "pagination").options;
        var pagingItems = $.data($context, "pagination").pagingItems;
        $.extend($opt, customOption || {});
        var ps = $($context).find("select.pagination-page-list");
        if (ps.length) {
            ps.val($opt.pageSize + "");
            $opt.pageSize = parseInt(ps.val());
        }
        var totalpages = Math.ceil($opt.total / $opt.pageSize) || 1;
        pagingItems.num.val($opt.pageNumber);
        pagingItems.after.html($opt.afterPageText.replace(/{pages}/, totalpages));
        var displayMsgFormat = $opt.displayMsg;
        displayMsgFormat = displayMsgFormat.replace(/{from}/, $opt.total == 0 ? 0 : $opt.pageSize * ($opt.pageNumber - 1) + 1);
        displayMsgFormat = displayMsgFormat.replace(/{to}/, Math.min($opt.pageSize * ($opt.pageNumber), $opt.total));
        displayMsgFormat = displayMsgFormat.replace(/{total}/, $opt.total);
        $($context).find("div.pagination-info").html(displayMsgFormat);
        pagingItems.first.add(pagingItems.prev).lnkbutton({
            disabled: ($opt.pageNumber == 1)
        });
        pagingItems.next.add(pagingItems.last).lnkbutton({
            disabled: ($opt.pageNumber == totalpages)
        });
        setLoading($context, $opt.loading);
    };
    function setLoading($context, isLoading) {
        var $opt = $.data($context, "pagination").options;
        var pagingItems = $.data($context, "pagination").pagingItems;
        $opt.loading = isLoading;
        if ($opt.showRefresh) {
            if ($opt.loading) {
                pagingItems.refresh.lnkbutton({
                    iconCls: "pagination-loading"
                });
            } else {
                pagingItems.refresh.lnkbutton({
                    iconCls: "pagination-load"
                });
            }
        }
    };
    $.fn.pagination = function (customOption, page) {
        if (typeof customOption == "string") {
            return $.fn.pagination.methods[customOption](this, page);
        }
        customOption = customOption || {};
        return this.each(function () {
            var finalOption;
            var $context = $.data(this, "pagination");
            if ($context) {
                finalOption = $.extend($context.options, customOption);
            } else {
                finalOption = $.extend({}, $.fn.pagination.defaults, $.fn.pagination.parseOptions(this), customOption);
                $.data(this, "pagination", {
                    options: finalOption
                });
            }
            preparePaging(this);
            prepareDisplayMsg(this);
        });
    };
    $.fn.pagination.methods = {
        options: function ($this) {
            return $.data($this[0], "pagination").options;
        },
        loading: function ($this) {
            return $this.each(function () {
                setLoading(this, true);
            });
        },
        loaded: function ($this) {
            return $this.each(function () {
                setLoading(this, false);
            });
        },
        refresh: function ($this, page) {
            return $this.each(function () {
                prepareDisplayMsg(this, page);
            });
        },
        select: function ($this, page) {
            return $this.each(function () {
                changePage(this, page);
            });
        }
    };
    $.fn.pagination.parseOptions = function ($this) {
        var t = $($this);
        return $.extend({}, $.parser.parseOptions($this, [{
            total: "number",
            pageSize: "number",
            pageNumber: "number"
        }, {
            loading: "boolean",
            showPageList: "boolean",
            showRefresh: "boolean"
        }]), {
            pageList: (t.attr("pageList") ? eval(t.attr("pageList")) : undefined)
        });
    };
    $.fn.pagination.defaults = {
        total: 1,
        pageSize: 10,
        pageNumber: 1,
        pageList: [10, 20, 30, 50],
        loading: false,
        buttons: null,
        showPageList: true,
        showRefresh: true,
        onSelectPage: function (pageNumber, pageSize) { },
        onBeforeRefresh: function (pageNumber, pageSize) { },
        onRefresh: function (pageNumber, pageSize) { },
        onChangePageSize: function (pageSize) { },
        onValidate: function () { return true; },
        beforePageText: "Page",
        afterPageText: "of {pages}",
        displayMsg: "Displaying {from} to {to} of {total} Designs",
        nav: {
            first: {
                iconCls: "pagination-first",
                handler: function () {
                    var $opt = $(this).pagination("options");
                    var $IsvalidToChange = $opt.onValidate.call(this);
                    if ($IsvalidToChange) {
                        if ($opt.pageNumber > 1) {
                            $(this).pagination("select", 1);
                        }
                    }
                }
            },
            prev: {
                iconCls: "pagination-prev",
                handler: function () {
                    var $opt = $(this).pagination("options");
                    var $IsvalidToChange = $opt.onValidate.call(this);
                    if ($IsvalidToChange) {                        
                        if ($opt.pageNumber > 1) {
                            $(this).pagination("select", $opt.pageNumber - 1);
                        }
                    }
                }
            },
            next: {
                iconCls: "pagination-next",
                handler: function () {
                    var $opt = $(this).pagination("options");
                    var $IsvalidToChange = $opt.onValidate.call(this);
                    if ($IsvalidToChange) {                        
                        var totalpages = Math.ceil($opt.total / $opt.pageSize);
                        if ($opt.pageNumber < totalpages) {
                            $(this).pagination("select", $opt.pageNumber + 1);
                        }
                    }
                }
            },
            last: {
                iconCls: "pagination-last",
                handler: function () {
                    var $opt = $(this).pagination("options");
                    var $IsvalidToChange = $opt.onValidate.call(this);
                    if ($IsvalidToChange) {                        
                        var totalpages = Math.ceil($opt.total / $opt.pageSize);
                        if ($opt.pageNumber < totalpages) {
                            $(this).pagination("select", totalpages);
                        }
                    }
                }
            },
            refresh: {
                iconCls: "pagination-refresh",
                handler: function () {
                    var $opt = $(this).pagination("options");
                    var $IsvalidToChange = $opt.onValidate.call(this);
                    if ($IsvalidToChange) {                        
                        if ($opt.onBeforeRefresh.call(this, $opt.pageNumber, $opt.pageSize) != false) {
                            $(this).pagination("select", $opt.pageNumber);
                            $opt.onRefresh.call(this, $opt.pageNumber, $opt.pageSize);
                        }
                    }                      
                }
            }
        }
    };
})(jQuery);