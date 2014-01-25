module.directive("addLineBtn", ['Scrap', function (Scrap) {
    return {
        restrict: "A",
        scope: {
            line: '=addLineBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                if(scope.line != null && scope.line.orderLineId && scope.line.notes!="" && scope.line.quantity && scope.line.price && scope.line.date){
                    Scrap.addLine(scope.line);
                    console.log(Scrap);
                    scope.line = null;
                	$('#error-line').html("");
                    scope.$apply();
                }else
                {
                	$('#err-line').html("Todos los campos son obligatorios");
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
}]).directive("viewOfferBtn", ['Scrap', '$location', function (Scrap, $location) {
    return {
        restrict: "A",
        scope: {
            offer: '=viewOfferBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                $location.path("/offer/" + scope.offer.id);
                scope.$apply();
            });
        }
    }
}]).directive("removeOfferBtn", ['Scrap', '$location', function (Scrap, $location) {
    return {
        restrict: "A",
        scope: {
            offer: '=removeOfferBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                Scrap.removeOffer(scope.offer.id);
                Scrap.getOffers();
            });
        }
    }
}]).directive('contenteditable', function () {
    return {
        restrict: 'A', // only activate on element attribute
        require: '?ngModel', // get a hold of NgModelController
        link: function (scope, element, attrs, ngModel) {
            if (!ngModel) return; // do nothing if no ng-model

            // Specify how UI should be updated
            ngModel.$render = function () {
                element.html(ngModel.$viewValue || '');
            };

            // Listen for change events to enable binding
            element.bind('blur keyup change', function () {
                scope.$apply(readViewText);
            });

            // No need to initialize, AngularJS will initialize the text based on ng-model attribute

            // Write data to the model
            function readViewText() {
                var html = element.html();

                // When we clear the content editable the browser leaves a <br> behind
                // If strip-br attribute is provided then we strip this out
                if (attrs.stripBr && html == '<br>') {
                    html = '';
                }
                ngModel.$setViewValue(html); //ngModel == linea.cantidad OR linea.descripcion
                console.log(ngModel);
            }
        }
    };
}).directive("viewPedidoBtn", ['Scrap', '$location', function (Scrap, $location) {
    return {
        restrict: "A",
        scope: {
            pedido: '=viewPedidoBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                $location.path("/orders-received/" + scope.pedido.id);
                scope.$apply();
            });
        }
    }
}]);