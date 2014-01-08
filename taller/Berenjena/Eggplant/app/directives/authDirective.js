module.directive("loginBtn", ['Auth', function (Auth) {
    return {
        restrict: "A",
        scope: {
            user: '=loginBtn'
        },
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                Auth.login(scope.user);
            });
        }
    }
}]).directive("logout", ['Auth', function (Auth) {
    return {
        restrict: "E",
        scope: {},
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                Auth.logout();
            });
        }
    }
}]).directive("username", ['Auth', function (Auth) {
    return {
        restrict: "E",
        scope: {},
        link: function (scope, element, attrs) {
            element.html(Auth.getUsername());
        }
    }
}]);
