$(document).ready(function () {
  $(".unauth-lang").click(function () {
    const lang = $(this).attr("data-lang");
    eraseCookie("lang");
    setCookie("lang", lang);
	location.reload();
  });
});
