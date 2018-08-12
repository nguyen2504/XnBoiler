(function() {
  app.directive('printInputCode', function () {
    return {
      restrict: 'AE',
      templateUrl: '/Angular/Directives/HomNay/InMdhNhap.html',
      scope: {
        infor: '=',
        maunhap: '=',
        nhaps: "=nhaps",
        ncc: '=',
        summoney: '=',
        tongncc: '=',
        vats:'='
      },
      link: function (scope,element, $http) {
      
        scope.tong = function (a, b) {
       
          return parseFloat(a) + parseFloat(b);
        }
     

        function sum(l, item) {

          var s = 0;
          if (item == 'vat') {
            var s = 0;
            for (var i = 0; i < l.length; i++) {
              s += l[i].vat * l[i].donGiaMua;
            }
          };
          if (item == 'sum') {
            for (var i = 0; i < l.length; i++) {
              s += (1 + l[i].vat) * l[i].donGiaMua * l[i].soLuong;
            }
          }
          return s;
        }
      }
    };
  });
})();



