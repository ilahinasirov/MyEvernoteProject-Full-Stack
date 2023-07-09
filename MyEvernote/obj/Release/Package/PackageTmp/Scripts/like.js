$(function () {

    var noteids = [];
    $("div[data-note-id]").each(function (i, e) {
        noteids.push($(e).data("note-id"));
    });

    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteids }
    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {

            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-note-id=" + id + "]");
                var btn = likedNote.find("button[data-liked]");
                var span = btn.find("span.like-heart"); //firstchild
                btn.data("liked", true);
                span.removeClass("fa-heart-o");
                span.addClass("fa-heart");
            }
        }
    }).fail(function () {

    });


    $("button[data-liked]").click(function () {
        var btn = $(this);
        var liked = btn.data("liked");
        var noteid = btn.data("note-id");
        var spanHeart = btn.find("span.like-heart");
        var spanCount = btn.find("span.like-count");

        $.ajax({
            method: "POST",
            url: "/Note/SetLikeState",
            data: { "noteid": noteid, "liked": !liked }
        }).done(function (data) {

            if (data.hasError) {
                alert(data.errorMessage);
            } else {
                liked = !liked;
                btn.data("liked", liked);
                spanCount.text(data.result);

                spanHeart.removeClass("fa-heart-o");
                spanHeart.removeClass("fa-heart");

                if (liked) {
                    spanHeart.addClass("fa-heart");
                } else {
                    spanHeart.addClass("fa-heart-o");

                }
            }

        }).fail(function () {
            alert("Could Not Connected Server! Please Log In And Try Again");
        });
    });
});