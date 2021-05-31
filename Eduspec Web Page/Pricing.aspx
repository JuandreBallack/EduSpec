<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="container">
        <h1>Pricing</h1>
        
        <div class="row">
            <div class="col-md-6">
                <h3 class="Eduspec_Green">Exemptions Module</h3>
                <%--<h4 class="Eduspec_Green">Annual subscription. (2020)</h4>
                <div class="well well-sm"><strong>R 2,050.00. (Excl. VAT)</strong> The annual subscription fee includes 20 applications.</div>     --%>           
                <%--<h3 class="Eduspec_Green">Exemptions Module</h3>--%>
                <h4 class="Eduspec_Green">Annual subscription.</h4>
                <div class="well well-sm"><strong>R 2,050.00. (Excl. VAT)</strong> The annual subscription fee includes 20 applications.</div>
                <h4 class="Eduspec_Green">Application packs.</h4>
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th>Package</th>
                            <th>Applications</th>
                            <th>Price (Exl. VAT)</th>
                            <th>Top Up 20</th>
                            <th>Top Up 50</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>AP50</td>
                            <td style="text-align:center">50</td>
                            <td>R 1,525.00</td>
                            <td>R 610.00</td>
                            <td>Not Applicable</td>
                        </tr>
                        <tr>
                            <td>AP100</td>
                            <td style="text-align:center">100</td>
                            <td>R 2,385.00</td>
                            <td>R 477.00</td>
                            <td>R 1,193.00</td>
                        </tr>
                        <tr>
                            <td>AP150</td>
                            <td style="text-align:center">150</td>
                            <td>R 2,940.00</td>
                            <td>R 392.00</td>
                            <td>R 980.00</td>
                        </tr>
                        <tr>
                            <td>AP200</td>
                            <td style="text-align:center">200</td>
                            <td>R 3,435.00</td>
                            <td>R 344.00</td>
                            <td>R 858.00</td>
                        </tr>
                        <tr>
                            <td>AP250</td>
                            <td style="text-align:center">250</td>
                            <td>R 3,760.00</td>
                            <td>R 300.00</td>
                            <td>R 752.00</td>
                        </tr>
                        <tr>
                            <td>AP300</td>
                            <td style="text-align:center">300</td>
                            <td>R 4,130.00</td>
                            <td>R 275.00</td>
                            <td>R 688.00</td>
                        </tr>
                        <tr>
                            <td>AP350</td>
                            <td style="text-align:center">350</td>
                            <td>R 4,525.00</td>
                            <td>R 258.00</td>
                            <td>R 646.00</td>
                        </tr>
                        <tr>
                            <td>AP400</td>
                            <td style="text-align:center">400</td>
                            <td>R 5,000.00</td>
                            <td>R 250.00</td>
                            <td>R 625.00</td>
                        </tr>                        
                    </tbody>
                </table>
                 <%--<div class="alert alert-danger" role="alert">* Please note that new activations will be accountable for an <strong>R 2,200.00(Excl. VAT) (2019)</strong> once of setup fee.</div>--%>
                 <div class="alert alert-danger" role="alert">* Please note that new activations will be accountable for an <strong>R 2,375.00(Excl. VAT)</strong> once of setup fee.</div>
             </div>
             <div class="col-md-4">
                <h3 class="Eduspec_Green">Application packs</h3>
                 <p>Application packs are acquired to fore fill the special need of each individual institution. You only need to purchase an application pack for the amount of application your institution does on an annual base. Application packs do not expire and can be carried over to the next year.</p>
                 <h3 class="Eduspec_Green">Top-Ups</h3>
                 <p>With applications for exemption on the increase at all institutions an institution can top-up their application packs in the same year that an application pack is a purchase.</p>
                 
            </div>
        </div>
        
        <div class="row">
            <div class="col-md-6">
                <h3 class="Eduspec_Green">Debt Management Module</h3>
                <h4 class="Eduspec_Green">Annual subscription.</h4>
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th>Number of learners</th>
                            <th>Price (Exl. VAT)</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Up to 249</td>
                            <td>R 2,650.00</td>
                        </tr>
                        <tr>
                            <td>250 - 499</td>
                            <td>R 3,550.00</td>
                        </tr>
                        <tr>
                            <td>500 and above</td>
                            <td>R 4,850.00</td>
                        </tr>
                     </tbody>
                   </table>
                </div>
            </div>
                
        <div class="row">
            <div class="col-md-6">
                <h3 class="Eduspec_Green">SMS Bundles</h3>

                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th style="width:150px">Bundle Description</th>
                            <th style="text-align:center; width:120px;">Quantity</th>
                            <th>Price (Exl. VAT)</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>B1000</td>
                            <td style="text-align:center">1000</td>
                            <td>R 360.00</td>
                        </tr>
                        <tr>
                            <td>B2000</td>
                            <td style="text-align:center">2000</td>
                            <td>R 690.00</td>
                        </tr>
                        <tr>
                            <td>B3000</td>
                            <td style="text-align:center">3000</td>
                            <td>R 990.00</td>
                        </tr>
                        <tr>
                            <td>B4000</td>
                            <td style="text-align:center">4000</td>
                            <td>R 1 260.00</td>
                        </tr>
                        <tr>
                            <td>B5000</td>
                            <td style="text-align:center">5000</td>
                            <td>R 1 500.00</td>
                        </tr>
                        <tr>
                            <td>B6000</td>
                            <td style="text-align:center">6000</td>
                            <td>R 1 710.00</td>
                        </tr>
                        <tr>
                            <td>B7000</td>
                            <td style="text-align:center">7000</td>
                            <td>R 1 890.00</td>
                        </tr>
                        <tr>
                            <td>B8000</td>
                            <td style="text-align:center">8000</td>
                            <td>R 2 040.00</td>
                        </tr>                        
                        <tr>
                            <td>B9000</td>
                            <td style="text-align:center">9000</td>
                            <td>R 2 160.00</td>
                        </tr>             
                        <tr>
                            <td>B10000</td>
                            <td style="text-align:center">10000</td>
                            <td>R 2 250.00</td>
                        </tr>                        
                    </tbody>
                </table>
                 <%--<div class="alert alert-danger" role="alert">* Please note that new activations will be accountable for an <strong>R 2,200.00(Excl. VAT)</strong> once of setup fee.</div>--%>
             </div>             
    </div>
        
    </div>    
</asp:Content>