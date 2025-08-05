$('.pagination a').click(function (e) {
    e.preventDefault();
    $('#result').load($(this).attr('href'));
});