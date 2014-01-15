module.directive("addLineBtn", ['Scrap', function (Scrap) {
    return {
        restrict: "A",
        scope: {
            line: '=addLineBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                if(scope.line.descripcion!="" && scope.line.cantidad && scope.line.precio){
                    Scrap.addLine(scope.line);
                    console.log(Scrap);
                    scope.line = null;
                    scope.$apply();
                }
            });
        }
    }
}]).directive("sendOfferBtn", ['Scrap', '$location', function (Scrap, $location) {
    return {
        restrict: "A",
        scope: {
            order: '=sendOrderBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                if (scope.order.id)
                {
                    Scrap.putOrder(scope.order);
                } else {
                    Scrap.postOrder(scope.order);
                }
                scope.order = [];
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