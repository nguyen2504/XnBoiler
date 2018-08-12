
app.factory('lastFistDate',function() {
  var factory = {};
  factory.last = function(y, m) {
    var lastDay = new Date(y, m, 0);
    return lastDay;
  };
  factory.first = function(y, m) {
    return new Date(y, m-1, 1);
  };
  factory.firstOrLastQui = function(y, m, b) {
    var t = m * 3;
    //console.log(t + ' ' + m);
    if (b == 0) {
      return new Date(y, t - 3, 1);
    } else {
      return new Date(y, t, 0);
    }
  };
    function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
  };
  factory.unique = function(l) {
    return  l.filter(onlyUnique);
  }
  return factory;
});
app.factory('fnFactory', function () {
  var factory = {};
  var ChuSo = new Array(" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín ");
  var Tien = new Array("", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ");
  factory.ingiay = function(divName, className) {
    var printContents = document.getElementById(divName).innerHTML;
    var popupWin = window.open('', '_blank', 'width=300,height=300');
    popupWin.document.open();
    popupWin.document.write('<html>' +
      '<head>' +
      ' <link href="/fonts/roboto/roboto.css" rel="stylesheet"  />' +
      '<link href = "/fonts/material-icons/materialicons.css" rel = "stylesheet"  />' +
      '<link href="/lib/bootstrap/dist/css/bootstrap.css" rel="stylesheet"  />' +
      '<link href="/lib/bootstrap-select/dist/css/bootstrap-select.css" rel="stylesheet"  />' +
      '<link href="/lib/toastr/toastr.css" rel="stylesheet"  />' +
      '<link href="/lib/famfamfam-flags/dist/sprite/famfamfam-flags.css" rel="stylesheet"  />' +
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
      ' <link href="/scss/' +
      className +
      '.css" rel="stylesheet"  />' +
      '</head>' +
      '<body onload="window.print()">' +
      printContents +
      '</body>' +
      '<script src="/js/table-angularjs/jquery.dataTables.min.js"></script>' +
      '</html>');
    popupWin.document.close();
    return "Xong";
  };
  function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
  };

  factory.unique = function(l) {
    return l.filter(onlyUnique);
  };
  factory.DocTienBangChu = function (SoTien) {
    var lan = 0;
    var i = 0;
    var so = 0;
    var KetQua = "";
    var tmp = "";
    var ViTri = new Array();
    if (SoTien < 0) return "Số tiền âm !";
    if (SoTien == 0) return "Không đồng !";
    if (SoTien > 0) {
      so = SoTien;
    }
    else {
      so = -SoTien;
    }
    if (SoTien > 8999999999999999) {
      //SoTien = 0;
      return "Số quá lớn!";
    }
    ViTri[5] = Math.floor(so / 1000000000000000);
    if (isNaN(ViTri[5]))
      ViTri[5] = "0";
    so = so - parseFloat(ViTri[5].toString()) * 1000000000000000;
    ViTri[4] = Math.floor(so / 1000000000000);
    if (isNaN(ViTri[4]))
      ViTri[4] = "0";
    so = so - parseFloat(ViTri[4].toString()) * 1000000000000;
    ViTri[3] = Math.floor(so / 1000000000);
    if (isNaN(ViTri[3]))
      ViTri[3] = "0";
    so = so - parseFloat(ViTri[3].toString()) * 1000000000;
    ViTri[2] = parseInt(so / 1000000);
    if (isNaN(ViTri[2]))
      ViTri[2] = "0";
    ViTri[1] = parseInt((so % 1000000) / 1000);
    if (isNaN(ViTri[1]))
      ViTri[1] = "0";
    ViTri[0] = parseInt(so % 1000);
    if (isNaN(ViTri[0]))
      ViTri[0] = "0";
    if (ViTri[5] > 0) {
      lan = 5;
    }
    else if (ViTri[4] > 0) {
      lan = 4;
    }
    else if (ViTri[3] > 0) {
      lan = 3;
    }
    else if (ViTri[2] > 0) {
      lan = 2;
    }
    else if (ViTri[1] > 0) {
      lan = 1;
    }
    else {
      lan = 0;
    }
    for (i = lan; i >= 0; i--) {
      tmp = DocSo3ChuSo(ViTri[i]);
      KetQua += tmp;
      if (ViTri[i] > 0) KetQua += Tien[i];
      if ((i > 0) && (tmp.length > 0)) KetQua += ',';//&& (!string.IsNullOrEmpty(tmp))
    }
    if (KetQua.substring(KetQua.length - 1) == ',') {
      KetQua = KetQua.substring(0, KetQua.length - 1);
    }
    KetQua = KetQua.substring(1, 2).toUpperCase() + KetQua.substring(2);
    return KetQua;//.substring(0, 1);//.toUpperCase();// + KetQua.substring(1);
  };
  function DocSo3ChuSo(baso) {
    var tram;
    var chuc;
    var donvi;
    var KetQua = "";
    tram = parseInt(baso / 100);
    chuc = parseInt((baso % 100) / 10);
    donvi = baso % 10;
    if (tram == 0 && chuc == 0 && donvi == 0) return "";
    if (tram != 0) {
      KetQua += ChuSo[tram] + " trăm ";
      if ((chuc == 0) && (donvi != 0)) KetQua += " linh ";
    }
    if ((chuc != 0) && (chuc != 1)) {
      KetQua += ChuSo[chuc] + " mươi";
      if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh ";
    }
    if (chuc == 1) KetQua += " mười ";
    switch (donvi) {
    case 1:
      if ((chuc != 0) && (chuc != 1)) {
        KetQua += " mốt ";
      }
      else {
        KetQua += ChuSo[donvi];
      }
      break;
    case 5:
      if (chuc == 0) {
        KetQua += ChuSo[donvi];
      }
      else {
        KetQua += " lăm ";
      }
      break;
    default:
      if (donvi != 0) {
        KetQua += ChuSo[donvi];
      }
      break;
    }
    return KetQua;
  }
  return factory;
});

