module.directive("addLineBtn", ['Scrap', function (Scrap) {
    return {
        restrict: "A",
        scope: {
            line: '=addLineBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                if(scope.line.orderLineId && scope.line.notes!="" && scope.line.quantity && scope.line.price){
                    Scrap.addLine(scope.line);
                    console.log(Scrap);
                    scope.line = null;
                    scope.$apply();
                }
            });
        }
    }
}]).directive("removeLineBtn", ['Scrap', function (Scrap) {
    return {
        restrict: "A",
        scope: {
            line: '=removeLineBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                Scrap.removeLine(scope.line);
                scope.$apply();
            });
        }
    }
}]).directive("sendOfferBtn", ['Scrap', '$location', function (Scrap, $location) {
    return {
        restrict: "A",
        scope: {
            offer: '=sendOfferBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                if (scope.offer.id)
                {
                    Scrap.putOffer(scope.offer);
                } else {
                    Scrap.postOffer(scope.offer);
                }
                $location.path("/orders");
                scope.$apply();
            });
        }
    }
}]).directive("viewOrderBtn", ['Scrap', '$location', function (Scrap, $location) {
    return {
        restrict: "A",
        scope: {
            order: '=viewOrderBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                $location.path("/order/" + scope.order.id);
                scope.$apply();
            });
        }
    }
}]);