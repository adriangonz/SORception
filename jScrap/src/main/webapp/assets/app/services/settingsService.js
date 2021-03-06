module.service( 'SettingsService', [ '$rootScope', '$http', function( $rootScope, $http ) {
   var service = {
	    settings: {
	     	"name": "",
	     	"validToken":{},
	     	"tokenList": {},
	     	"userList": {},
	 		},
	 
	    getSettings: function () {
	        $http({method: 'GET', url: '/jScrap/api/settings'}).
			  success(function(data, status, headers, config) {
	       		angular.extend(service.settings, data);
	       		$rootScope.$broadcast( 'settings.update' );
			  }).
			  error(function(data, status, headers, config) {
			  	alert(status+" | "+data);
	       		$rootScope.$broadcast( 'settings.update' );
			  });    	
	    },

	    postSettings: function(){
	     	  $http({method: 'POST', url: '/jScrap/api/token'}).
	          success(function(data, status, headers, config) {
	            service.getSettings();
	          }).
	          error(function(data, status, headers, config) {
	            alert(status+" | "+data);
	          });
	    },

	    getUsers: function () {
	        $http({method: 'GET', url: '/jScrap/api/user'}).
			  success(function(data, status, headers, config) {
	       		service.settings.userList=data;
	       		$rootScope.$broadcast( 'settings.update' );
			  }).
			  error(function(data, status, headers, config) {
			  	alert(status+" | "+data);
	       		$rootScope.$broadcast( 'settings.update' );
			  });    	
	    },

	   	postUser: function(user_data){
	     	  $http({method: 'POST', url: '/jScrap/api/user', data: user_data}).
	          success(function(data, status, headers, config) {
	            service.getUsers();
	          }).
	          error(function(data, status, headers, config) {
	            alert(status+" | "+data);
	          });
	    },
   	}
 
   return service;
}]);

