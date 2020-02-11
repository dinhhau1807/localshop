$(document).ready(function () {
    function redirect() {
        var view = $('#viewSort').children('option:selected').val();
        var sortBy = $('#sortBy').children('option:selected').val();

        if (view == 20 && sortBy == '@SortByEnums.Default') {
            window.location.href = `/shop`;
        } else if (view != 20 && sortBy == '@SortByEnums.Default') {
            window.location.href = `/shop?view=${view}`;
        } else if (view == 20 && sortBy != '@SortByEnums.Default') {
            window.location.href = `/shop?sortBy=${sortBy}`;
        } else {
            window.location.href = `/shop?view=${view}&sortBy=${sortBy}`;
        }
    }

    $('#viewSort').on('change', function () {
        redirect();
    });
    $('#sortBy').on('change', function () {
        redirect();
    })

});