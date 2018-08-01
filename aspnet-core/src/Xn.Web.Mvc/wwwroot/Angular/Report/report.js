(function() {
  app.controller('ctrl', ['$scope', '$http', 'lastFistDate','$filter', fn]);
  function fn($scope, $http, lastFistDate, $filter) {
    ////console.log('mdh ' + localStorage.getItem("mdh"));
    $scope.getNhap = function () {
      var mdh = localStorage.getItem("mdh");
      localStorage.removeItem("mdh");
      var url = "Report/Ins";
      //$scope.searching = "All";
      if (mdh != null) {
        $scope.searching = mdh;
        url = "Report/Ins?mdh="+mdh;
      }
      $http.post(url).then(function (e) {
        $scope.initNhaps(e);
      });
    };
    $scope.onBlank = function() {

      $scope.searching = '';
    };
    function findObjectByKey(array, key, value) {
      for (var i = 0; i < array.length; i++) {
        if (array[i][key] === value) {
          return array[i];
        }
      }
      return null;
    }
    $scope.initNhaps = function (e) {
      $scope.nhaps = [];
      $scope.nhaps = e.data.result;
      var t = [];
      for (var i = 0; i < $scope.nhaps.length; i++) {
        t.push($scope.nhaps[i].tenNcc);
      }
      var l = lastFistDate.unique(t);
      for (var i = 0; i <= l.length; i++) {
        console.log(l[i]);
        var newTemp = $filter("filter")($scope.nhaps, { tenNcc: l[i] });
        console.log(newTemp.length);
      }
    }
    // 
    $scope.getNhaps = function() {
      var url = "Report/Ins";
      $http.post(url).then(function (e) {
        $scope.initNhaps(e);
      });
    }
    //
    $scope.export = function(filename) {
      var url = "Report/OnPostExport";
      var data = JSON.parse(localStorage.getItem('get'));
      console.log(JSON.stringify(data));
    
      $http.post(url,data).then(function (e) {
       
        window.location.href = e.data;
      });
    
    }
    //
    $scope.getThangs = function(m) {
      $scope.first = lastFistDate.first($scope.nam, m);
      $scope.last = lastFistDate.last($scope.nam, m);
      $scope.onDate();
    };
    $scope.getQuis = function(m) {
      $scope.first = lastFistDate.firstOrLastQui($scope.nam, m,0);
      $scope.last = lastFistDate.firstOrLastQui($scope.nam, m,1);
      $scope.onDate();
    }
    $scope.onDate = function() {
      //console.log($scope.first + " " + $scope.last.toLocaleDateString());
      var url = "Report/GetSearching";
      var data = {
        name: $scope.searching ,
        maDh: $scope.maDh,
        begin: $scope.first,
        end: $scope.last,
        memory: -1
      }
      localStorage.setItem('get', JSON.stringify(data));
      $http.post(url, data).then(function (e) {
        $scope.initNhaps(e);
        
      });
    }
    $scope.onTenHang = function() {
      var url = "Report/OnTenHang";
      var data = {
        name: $scope.tenHang,
        maDh: $scope.maDh,
        begin: $scope.first,
        end: $scope.last,
        memory:0
      }
      localStorage.setItem('get', JSON.stringify(data));
      $http.post(url, data).then(function (e) {
        $scope.initNhaps(e);
        $scope.donHang = "All";
        $scope.ncc = "All";
      });
    }
    $scope.onNcc = function () {
      var url = "Report/OnNcc";
      var data = {
        name: $scope.ncc,
        maDh: $scope.maDh,
        begin: $scope.first,
        end: $scope.last,
        memory:2
      }
      localStorage.setItem('get', JSON.stringify(data));
      $http.post(url, data).then(function (e) {
        $scope.initNhaps(e);
        $scope.donHang = "All";
        $scope.tenHang = "All";
      });
    }
    $scope.onSearch = function () {
      var url = "Report/OnSearch";
      var data = {
        name: $scope.searching,
        maDh: $scope.maDh,
        begin: $scope.first,
        end: $scope.last,
        memory:3
      }
      localStorage.setItem('get', JSON.stringify(data));
      $http.post(url, data).then(function (e) {
        $scope.initNhaps(e);
        $scope.donHang = "All";
        $scope.tenHang = "All";
      });
    }
    $scope.onDonHang = function () {
      var url = "Report/OnDonHang";
      var data = {
        name: $scope.donHang,
        maDh: $scope.maDh,
        begin: $scope.first,
        end: $scope.last,
        memory:1
      }
      localStorage.setItem('get', JSON.stringify(data));
      $http.post(url, data).then(function (e) {
        $scope.initNhaps(e);
        $scope.ncc=  $scope.tenHang = "All";
      });
    }
    $scope.onSelectMaDh = function() {
      var url = "Nhap/GetSearching";
      var data = {
        name: 'All',
        maDh: $scope.maDh,
        begin: $scope.first,
        end: $scope.last
      }

      $http.post(url, data).then(function (e) {
        $scope.initNhaps(e);
        //console.log(JSON.stringify($scope.nhaps));
      });
    }
    $scope.onSelectNcc = function() {
      var url = "Nhap/GetSearching";
      var data = {
        name: 'All',
        maDh: $scope.maDh,
        begin: $scope.first,
        end: $scope.last
      }

      $http.post(url, data).then(function (e) {
        $scope.initNhaps(e);
       
      });
    }
    $scope.onClickmaDh = function(a,b) {
   
      var url = "Report/OnDonHang";
     if (a == 1) {
        url = "Report/OnTenHang";
      }
      var data = {
        name: b,
        maDh: $scope.maDh,
        begin: $scope.first,
        end: $scope.last,
        memory: 1
      }
      localStorage.setItem('get', JSON.stringify(data));
      $http.post(url, data).then(function (e) {
        $scope.initNhaps(e);
        $scope.ncc = $scope.tenHang = "All";
      });
    }
    function initThang() {
      $scope.thangs = [];
      for (var i = 1; i <= 12; i++) {
        $scope.thangs.push(i);
      }
    }
    function initQui(nam) {
      $scope.quis = [];
      for (var i = 1; i <= 4; i++) {
        $scope.quis.push(i);
      }
    }
    $scope.onNam = function() {
      initQui($scope.nam);
    }
    $scope.maDonhangOrtenHangs = function() {
      $scope.searchings = [];
      var url = "Nhap/GetMaDonhangOrtenHangs";
      $http.post(url).then(function (e) {
       
        $scope.searchings = e.data.result;
        console.log(JSON.stringify($scope.searchings));
      });
    }
    $scope.getNccs = function() {
      var url = "Ncc/GetNameNccs";
      $http.post(url).then(function (e) {
        $scope.nccs = e.data.result;
      });
    }

    function initNams() {
      $scope.nams = [];
      var d = new Date();
      var n = d.getFullYear();
      $scope.nam = n;
      for (var i = n - 10; i <= n+12; i++) {
        $scope.nams.push(i);
      }
    }

    $scope.printToCart = function (printSectionId) {

      var innerContents = document.getElementById(printSectionId).innerHTML;
      var popupWinindow = window.open('',
        '_blank',
        'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
      popupWinindow.document.open();
      popupWinindow.document.write(
        '<html>' +
        '<head>' +
        '<link rel="stylesheet" type="text/css" href="/css/testPrint.css" />' +
        '</head>' +
        '<body onload="window.print()">' +
        innerContents +
        '</body>' +
        '</html>');
      popupWinindow.document.close();
    };
    $scope.printDiv = function(divName) {
      var printContents = document.getElementById(divName).innerHTML;
      var popupWin = window.open('', '_blank', 'width=300,height=300');
      popupWin.document.open();
      popupWin.document.write('<html>' +
        '<head>' +
        ' <link href="/fonts/roboto/roboto.css" rel="stylesheet"  />' +
        '<link href = "/fonts/material-icons/materialicons.css" rel = "stylesheet"  />' +
        '<link href="/lib/bootstrap/dist/css/bootstrap.css" rel="stylesheet"  />' +
        '<link href="/lib/bootstrap-select/dist/css/bootstrap-select.css" rel="stylesheet"  />'+
        '<link href="/lib/toastr/toastr.css" rel="stylesheet"  />'+
        '<link href="/lib/famfamfam-flags/dist/sprite/famfamfam-flags.css" rel="stylesheet"  />'+
        '<link href="/lib/font-awesome/css/font-awesome.css" rel="stylesheet"  />' +
        '<link href="/lib/Waves/dist/waves.css" rel="stylesheet"  />' +
        '<link href="/lib/animate.css/animate.css" rel="stylesheet"  />' +
        ' <link href="/css/materialize.css" rel="stylesheet"  />' +
        ' <link href="/css/style.css" rel="stylesheet" >' +
        ' <link href="/css/styleXns.css" rel="stylesheet" >' +
        '<link href="/css/testPrint.css" rel="stylesheet" >' +
        ' <link href="/css/themes/all-themes.css" rel="stylesheet"  />' +
        ' <link href="/view-resources/Views/Shared/_Layout.css" rel="stylesheet"  />' +
        ' <link href="/js/table-angularjs/jquery.dataTables.min.css" rel="stylesheet" >' +
        ' <link href="/scss/snackbar.css" rel="stylesheet"  />' +
        '</head>' +
        '<body onload="window.print()">' + printContents + '</body>' +
        '<script src="/js/table-angularjs/jquery.dataTables.min.js"></script>'
         +
        '</html>');
      popupWin.document.close();
    } 
    function init() {
      var n = new Date().getFullYear();
      $scope.getNhap();
      initThang();
      initQui(n);
      initNams();
      $scope.getNccs();
      $scope.first = $scope.last = new Date();
      //$scope.maDonhangOrtenHangs();
    };
   
    init();
    // print
    $scope.printName = "What your năme";
    $scope.Ngay = new Date().toUTCString();
    $scope.loadCty = function() {
      var url = "Report/CongTy";
      $http.post(url).then(function (e) {
        $scope.inforCty = e.data.result;
      });
    };
    $scope.mauNhap = "Mẫu số :  02-VT (Ban hành theo QĐ 15/2006/QĐ-BTC ngày 20/03/2006 của Bộ Trưởng Bộ Tài Chính)";
    function initPrint() {
      $scope.loadCty();
     
    };

    initPrint();
  };
})();
(function() {
  app.config(function($routeProvider) {
    $routeProvider
      .when("/", {
        templateUrl : "Angular/Report/nhaphang.html"
      })
      .when("/n", {
        templateUrl : "Angular/Report/nhaphang.html"
      })
      .when("/x", {
        templateUrl : "Angular/Report/xuathang.html"
      })
      .when("/p", {
        templateUrl: "Angular/Report/print.html"
      });
  });
})();
