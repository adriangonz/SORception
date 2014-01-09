module.directive("addLineBtn", ['Taller', function (Taller) {
    return {
        restrict: "A",
        scope: {
            line: '=addLineBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                if(scope.line.descripcion!="" && scope.line.cantidad){
                    Taller.addLine(scope.line);
                    scope.line = null;
                    scope.$apply();
                }
            });
        }
    }
}]).directive("sendOrderBtn", ['Taller', function (Taller) {
    return {
        restrict: "A",
        scope: {
            order: '=sendOrderBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                Taller.sendOrder(scope.order);
                scope.order = [];
                $location.path("/orders");
                scope.$apply();
            });
        }
    }
}]).directive("viewOrderBtn", ['Taller', '$location', function (Taller, $location) {
    return {
        restrict: "A",
        scope: {
            order: '=viewOrderBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                alert("order_id: "+scope.order.id);
                $location.path("/order/" + scope.order.id);
                scope.$apply();
            });
        }
    }
}]);