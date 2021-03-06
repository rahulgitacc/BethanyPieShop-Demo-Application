﻿function LoadMorePies(skipItems) {
    
    $.ajax({
        type: 'GET',
        url: '/api/PieData',
        data: { pieId: skipItems },
        dataType: 'json',
        success: function (jsonData) {
            if (jsonData === null) {
                alert('no data returned');
                return;
            }
            $.each(jsonData, function (index, pie) {

                var PieSummarString = '<div class="col-sm-4 col-lg-4 col-md-4"> ' +
                    '  <div class="thumbnail">' +
                    '     <img src="' + pie.imageThumbnailUrl + '" alt="">' +
                    '      <div class="caption">' +
                    '         <h3 class="pull-right">' + pie.price + '</h3>' +
                    '         <h3>' +
                    '             <a href=/Pie/Details/' + pie.pieId + '>' + pie.name + '</a>' +
                    '         </h3>' +
                    '         <p>' + pie.shortDescription + '</p>' +
                    '    </div>' +
                    '    <div class="addToCart">' +
                    '        <p class="button">' +
                    '             <a class="btn btn-primary" href=/ShoppingCart/AddToShoppingCart?pieId=' + pie.pieId + '>Add to cart</a>' +
                    '         </p>' +
                    '     </div>' +
                    '  </div>' +
                    '</div>';

                $('#pieDiv').append(PieSummarString);
            });
        },
        error: function (ex) {
            alert(ex);
        }
    });
    return false;
}