module.service('Auth', ['$rootScope', '$http', '$location', '$cookies', function ($rootScope, $http, $location, $cookies) {
   var service = {
	 
	    login: function (user) {	
	        $http({method: 'POST', url: '/jScrap/api/user/authenticate', data: user}).
			  success(function(data, status, headers, config) {
			      $cookies["SessionScrap"] = data;
			      console.log(data);
			      $http.defaults.headers.common.Authorization = data;
			    $rootScope.$broadcast('auth.login');
	       		$location.path("/orders");
			  }).
			  error(function(data, status, headers, config) {
			  	console.log(status+" | "+data);
	       		$location.path("/login");
			  });    	
	    },

	    logout: function(){
	     	  $http({method: 'POST', url: '/jScrap/api/logout'}).
	          success(function(data, status, headers, config) {
	              delete $cookies["SessionScrap"];
	             $http.defaults.headers.common.Authorization = ""; 
	              $rootScope.$broadcast('auth.login');
	       		$location.path("/login");
	          }).
	          error(function(data, status, headers, config) {
	            alert(status+" | "+data);
	          });
	    },

	    isLoggedIn: function () {
	    	$rootScope.$broadcast('auth.login');
	        SessionScrap = $cookies["SessionScrap"];
	        if (SessionScrap) {
			      $http.defaults.headers.common.Authorization = SessionScrap;
                return true;
            }
	       	$location.path("/login");
        },
   	}
 
   return service;
}]);

