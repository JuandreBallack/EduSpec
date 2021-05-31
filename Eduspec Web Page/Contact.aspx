<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

    <div class="container">
        <h1>Contact Form</h1>
        <div class="row">
            <div class="col-md-6">
                <div class="messages"></div>
                <div class="controls">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="form_name" class="Eduspec_Green">Your Name (required)</label>
                                <input id="form_name" type="text" name="name" class="form-control" placeholder="Please enter your name *" required="required" data-error="Firstname is required."/>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="form_school" class="Eduspec_Green">Institution (required)</label>
                                <input id="form_school" type="tel" name="school" class="form-control " placeholder="Please enter your institution *" required="required" data-error="Institution is required."/>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div> 
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="form_email" class="Eduspec_Green">Email (required)</label>
                                <input id="form_email"  runat="server" type="email" name="email" class="form-control" placeholder="Please enter your email *" required="required" data-error="Valid email is required."/>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="form_phone" class="Eduspec_Green">Phone</label>
                                <input id="form_phone" type="tel" name="phone" class="form-control " placeholder="Please enter your phone *" required="required" data-error="Phone number is required."/>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <div class="g-recaptcha" data-sitekey="6LcUomYUAAAAAJ5TmN8V_xPbSQp3ZHwOTdI51UQg" data-callback="enableBtn"> </div>
                                <%--<input class="form-control d-none" data-recaptcha="true" required="required" data-error="Please complete the Captcha"/>--%>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="form_message" class="Eduspec_Green">Message (required)</label>
                                <textarea id="form_message" name="message" class="form-control" placeholder="Message for me *" rows="8" required="required" data-error="Please,leave us a message."></textarea>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <input id="BtnSent" type="submit" runat="server" onserverclick="SentContact" class="btn btn-success btn-send" value="Send message"/>
                        </div>
                    </div>                    
                </div>
            </div>
            <div class="col-md-6">
                <iframe src="https://maps.google.co.za/maps?ie=UTF8&amp;q=12+Rooibok+Street+Rand en dal&amp;gl=ZA&amp;hl=en_uk&amp;t=m&amp;ll=-26.081346, 27.773109&amp;spn=0.007726,0.017145&amp;z=16&amp;iwloc=A&amp;output=embed" width="500" height="350" style="border:1px"></iframe>
            </div>
        </div>
    </div>
    <br />

<script type="text/javascript">

    //document.getElementById("BtnSent").disabled = true;
    window.onload = function () {
        var e = document.getElementById("Content_BtnSent");
        e.disabled = true;
    }

    function enableBtn() {
        document.getElementById("Content_BtnSent").disabled = false;
    }

</script>
    
</asp:Content>