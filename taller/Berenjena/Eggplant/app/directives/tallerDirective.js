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
                    console.log(Taller);
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
                if (scope.order.id)
                {
                    Taller.putOrder(scope.order);
                } else {
                    Taller.postOrder(scope.order);
                }
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
                alert("order_id: " + scope.order.id);
                $location.path("/order/" + scope.order.id);
                scope.$apply();
            });
        }
    }
}]).directive("removeLineBtn", ['Taller', function (Taller) {
    return {
        restrict: "A",
        scope: {
            line: '=removeLineBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                Taller.removeLine(scope.line);
                scope.$apply();
            });
        }
    }
}]).directive("editOrderBtn", ['Taller', '$location', function (Taller, $location) {
    return {
        restrict: "A",
        scope: {
            order: '=editOrderBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                Taller.actual_order = scope.order;
                $location.path("/edit-order");
                scope.$apply();
            });
        }
    }
}]).directive("removeOrderBtn", ['Taller', '$location', function (Taller, $location) {
    return {
        restrict: "A",
        scope: {
            order: '=removeOrderBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                Taller.removeOrder(scope.order.id);
                $location.path("/orders");
            });
        }
    }
}]);