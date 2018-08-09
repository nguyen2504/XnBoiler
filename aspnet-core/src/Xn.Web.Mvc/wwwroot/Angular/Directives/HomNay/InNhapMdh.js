
app.directive('printInputCode', function() {
  return {
    restrict: 'AE',
    templateUrl: '/Angular/Directives/HomNay/InMdhNhap.html',
    scope: {
      infor: '=',
      maunhap: '=',
      nhaps: '='
    },
    link: function (scope, element, $http) {
      scope.$watch("scope.nhaps", function () {
        console.log("ok");
      });

    }
  };
});

function fn($scope) {
    return {
      restrict:'AE',
      templateUrl:'/Angular/Directives/HomNay/InMdhNhap.html',
     scope: {
       infor: '=',
       maunhap: '=',
       nhaps: '='
      },
      link: function (scope, element) {
    
        $scope.$watch('nhaps', function () {
          console.log('aaaaaaaaaaaaaaaaa');
          console.log(JSON.stringify(scope.nhaps));
        });
      
      }
    };
  };
