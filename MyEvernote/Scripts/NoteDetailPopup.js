$(function () {

    $("#modal_note").on('show.bs.modal',
        function (e) {

            var btn = $(e.relatedTarget);
            var noteid = btn.data("note-id");

            $("#modal_note_body").load("/Note/GetNoteText/" + noteid);
        });

});