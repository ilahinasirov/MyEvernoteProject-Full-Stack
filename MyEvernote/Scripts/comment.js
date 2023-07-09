var noteid = -1;
var modalCommentBodyId = "#modal_comment_body";

$(function () {
    $('#modal_comment').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        noteid = btn.data("note-id");

        $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
    });
});

function doComment(btn, e, commentid, spanid) {

    var button = $(btn);
    var mode = button.data("edit-mode");

    if (e === "edit_clicked") {

        var btnSpan;
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            btnSpan = button.find("span");
            btnSpan.removeClass("fa-edit");
            btnSpan.addClass("fa-check");

            $(spanid).addClass("editable");
            $(spanid).attr("contenteditable", true);
            $(spanid).focus();
        }
        else {
            button.data("edit-mode", false);
            button.addClass("btn-warning");
            button.removeClass("btn-success");
            btnSpan = button.find("span");
            btnSpan.addClass("fa-edit");
            btnSpan.removeClass("fa-check");

            $(spanid).removeClass("editable");
            $(spanid).attr("contenteditable", false);

            var txt = $(spanid).text();

            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentid,
                data: { text: txt }
            }).done(function (data) {

                if (data.result) {
                    // comment partial tekrar yuklensin
                    $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
                }
                else {
                    alert("Comment Could Not Updated");
                }
            }).fail(function () {
                alert("Could Not Connected Server");
            });
        }


    }

    else if (e === "delete_clicked") {

        var dialog_res = confirm("Are You Sure Delete This Comment");
        if (!dialog_res) return false;

        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentid

        }).done(function (data) {
            if (data.result) {
                // comment partial tekrar yuklensin
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Comment Could Not Deleted");
            }

        }).fail(function () {
            alert("Could Not Connected Server");
        });


    }
    else if (e === "new_clicked") {
        var txt = $("#new_comment_text").val();

        $.ajax({
            method: "POST",
            url: "/Comment/Create",
            data: { "text": txt, "noteid": noteid }

        }).done(function (data) {
            if (data.result) {
                // comment partial tekrar yuklensin
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Comment Could Not Added");
            }

        }).fail(function () {
            alert("Could Not Connected Server");
        });
    }
}