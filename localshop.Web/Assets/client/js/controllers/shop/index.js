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
    });

    $('#shop1').on('click', function (e) {
        e.preventDefault();
        $(this).prev().val('Default');
        $(this).closest('form').submit();
    });
    $('#shop2').on('click', function (e) {
        e.preventDefault();
        $(this).prev().prev().val('List');
        $(this).closest('form').submit();
    });
});