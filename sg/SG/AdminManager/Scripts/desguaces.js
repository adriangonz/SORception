function changeStatus(elem, id) {
    elem.prop('disabled', true);
    $.ajax({
        url: "/api/desguace/"+id,
        type: 'PUT',
        data: { "active": elem.val() }
    }).done(function () {
        console.log("#row" + id);
        $("#row" + id)
            .animate({ backgroundColor: "#B2DBA1" }, 2000)
            .animate({ backgroundColor: "#FFFFFF" }, 1000);
        //$("#succes").fadeIn().delay(5000).fadeOut();
        elem.prop('disabled', false);
    }).error(function (error) {
        $("#row" + id)
            .animate({ backgroundColor: "#DCA7A7" }, 1000)
            .animate({ backgroundColor: "#FFFFFF" }, 1000);
        $("#error").html("Error: "+error.responseText);
        $("#error").fadeIn().delay(5000).fadeOut();
        elem.prop('disabled', false);
    });
}