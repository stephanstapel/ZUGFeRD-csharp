<?xml version="1.0" encoding="UTF-8"?>
<!-- 

Nutzungsrechte 
ZUGFeRD Datenformat Version 1.0, 25.6.2014

Zweck des Forums für elektronische Rechnungen bei der AWV e.V („FeRD“) ist u.a. die Schaffung und Spezifizierung 
eines offenen Datenformats für strukturierten elektronischen Datenaustausch auf der Grundlage offener und nicht 
diskriminierender, standardisierter Technologien („ZUGFeRD Datenformat“)

Das ZUGFeRD Datenformat wird nach Maßgabe des FeRD sowohl Unternehmen als auch der öffentlichen Verwaltung 
frei zugänglich gemacht. Hierfür bietet FeRD allen Unternehmen und Organisationen der öffentlichen Verwaltung eine 
Lizenz für die Nutzung des urheberrechtlich geschützten ZUGFeRD-Datenformats zu fairen, sachgerechten und nicht 
diskriminierenden Bedingungen an.

Die Spezifikation des FeRD zur Implementierung des ZUGFeRD Datenformats ist in ihrer jeweils geltenden Fassung 
abrufbar unter www.ferd-net.de.

Im Einzelnen schließt die Nutzungsgewährung ein: 
=====================================

FeRD räumt eine Lizenz für die Nutzung des urheberrechtlich geschützten ZUGFeRD Datenformats in der jeweils 
geltenden und akzeptierten Fassung (www.ferd-net.de) ein. 
Die Lizenz beinhaltet ein unwiderrufliches Nutzungsrecht einschließlich des Rechts der Weiterentwicklung, 
Weiterbearbeitung und Verbindung mit anderen Produkten.
Die Lizenz gilt insbesondere für die Entwicklung, die Gestaltung, die Herstellung, den Verkauf, die Nutzung oder 
anderweitige Verwendung des ZUGFeRD Datenformats für Hardware- und/oder Softwareprodukte sowie sonstige 
Anwendungen und Dienste. 
Diese Lizenz schließt nicht die wesentlichen Patente der Mitglieder von FeRD ein. Als wesentliche Patente sind Patente 
und Patentanmeldungen weltweit zu verstehen, die einen oder mehrere Patentansprüche beinhalten, bei denen es sich um 
notwendige Ansprüche handelt. Notwendige Ansprüche sind lediglich jene Ansprüche der Wesentlichen Patente, die durch 
die Implementierung des ZUGFeRD Datenformats notwendigerweise verletzt würden. 
Der Lizenznehmer ist berechtigt, seinen jeweiligen Konzerngesellschaften ein unbefristetes, weltweites, nicht übertragbares, 
unwiderrufliches Nutzungsrecht einschließlich des Rechts der Weiterentwicklung, Weiterbearbeitung und Verbindung mit 
anderen Produkten einzuräumen. 

Die Lizenz wird kostenfrei zur Verfügung gestellt. 

Außer im Falle vorsätzlichen Verschuldens oder grober Fahrlässigkeit haftet FeRD weder für Nutzungsausfall, entgangenen 
Gewinn, Datenverlust, Kommunikationsverlust, Einnahmeausfall, Vertragseinbußen, Geschäftsausfall oder für Kosten, 
Schäden, Verluste oder Haftpflichten im Zusammenhang mit einer Unterbrechung der Geschäftstätigkeit, noch für konkrete, 
beiläufig entstandene, mittelbare Schäden, Straf- oder Folgeschäden und zwar auch dann nicht, wenn die Möglichkeit der 
Kosten, Verluste bzw. Schäden hätte normalerweise vorhergesehen werden können.

-->
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:altova="http://www.altova.com" xmlns:altovaext="http://www.altova.com/xslt-extensions" xmlns:clitype="clitype" xmlns:clm5ISO42173A="urn:un:unece:uncefact:codelist:standard:5:ISO42173A:2010-04-07" xmlns:clm6Recommendation20="urn:un:unece:uncefact:codelist:standard:6:Recommendation20:6" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:iso4217="http://www.xbrl.org/2003/iso4217" xmlns:ix="http://www.xbrl.org/2008/inlineXBRL" xmlns:java="java" xmlns:link="http://www.xbrl.org/2003/linkbase" xmlns:qdt="urn:un:unece:uncefact:data:standard:QualifiedDataType:2" xmlns:ram="urn:un:unece:uncefact:data:draft:ReusableAggregateBusinessInformationEntity:1" xmlns:rsm="urn:un:unece:uncefact:data:standard:CBFBUY:5" xmlns:sps="http://www.altova.com/StyleVision/user-xpath-functions" xmlns:udt="urn:un:unece:uncefact:data:standard:UnqualifiedDataType:9" xmlns:xbrldi="http://xbrl.org/2006/xbrldi" xmlns:xbrli="http://www.xbrl.org/2003/instance" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="#all">
	<xsl:output version="4.0" method="html" indent="no" encoding="UTF-8" use-character-maps="spaces" doctype-public="-//W3C//DTD HTML 4.01 Transitional//EN" doctype-system="http://www.w3.org/TR/html4/loose.dtd"/>
	<xsl:character-map name="spaces">
		<xsl:output-character character="&#160;" string="&amp;nbsp;"/>
	</xsl:character-map>
	<xsl:param name="altova:bGeneratingFromPxf" select="false()"/>
	<xsl:param name="SV_OutputFormat" select="'HTML'"/>
	<xsl:param name="SV_BaseOutputFileName" as="xs:string?">
		<xsl:sequence select="for $i in altovaext:get-base-output-uri(), $j in tokenize( $i, &apos;[/\\]&apos; )[last()] return replace( $j, &apos;\.[^\.\s#%;]*$&apos;, &apos;&apos; )" use-when="function-available(&apos;altovaext:get-base-output-uri&apos;)"/>
	</xsl:param>
	<xsl:param name="SV_GeneratedFileNamePrefix" select="if ( $SV_BaseOutputFileName ) then $SV_BaseOutputFileName else &apos;zugferd_rc_s1p0_c1p0&apos;" as="xs:string?"/>
	<xsl:variable name="XML" select="/"/>
	<xsl:variable name="altova:nPxPerIn" select="96"/>
	<xsl:decimal-format name="format1" grouping-separator="." decimal-separator=","/>
	<xsl:import-schema schema-location="ZUGFeRD_RC.xsd" use-when="system-property('xsl:is-schema-aware')='yes'" namespace="urn:un:unece:uncefact:data:standard:CBFBUY:5"/>
	<xsl:variable name="altova:CssImages" select="()"/>
	<xsl:template match="/">
		<xsl:call-template name="altova:Root"/>
	</xsl:template>
	<xsl:template name="altova:Root">
		<html>
			<head>
				<title/>
				<meta http-equiv="X-UA-Compatible" content="IE=9"/>
				<xsl:comment>[if IE]&gt;&lt;STYLE type=&quot;text/css&quot;&gt;.altova-rotate-left-textbox{filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3)} .altova-rotate-right-textbox{filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=1)} &lt;/STYLE&gt;&lt;![endif]</xsl:comment>
				<xsl:comment>[if !IE]&gt;&lt;!</xsl:comment>
				<style type="text/css">.altova-rotate-left-textbox{-webkit-transform: rotate(-90deg) translate(-100%, 0%); -webkit-transform-origin: 0% 0%;-moz-transform: rotate(-90deg) translate(-100%, 0%); -moz-transform-origin: 0% 0%;-ms-transform: rotate(-90deg) translate(-100%, 0%); -ms-transform-origin: 0% 0%;}.altova-rotate-right-textbox{-webkit-transform: rotate(90deg) translate(0%, -100%); -webkit-transform-origin: 0% 0%;-moz-transform: rotate(90deg) translate(0%, -100%); -moz-transform-origin: 0% 0%;-ms-transform: rotate(90deg) translate(0%, -100%); -ms-transform-origin: 0% 0%;}</style>
				<xsl:comment>&lt;![endif]</xsl:comment>
				<style type="text/css">@page { margin-left:0.60in; margin-right:0.60in; margin-top:0.79in; margin-bottom:0.79in } @media print { br.altova-page-break { page-break-before: always; } }</style>
			</head>
			<body style="font-family:Courier; font-size:small; ">
				<xsl:for-each select="$XML">
					<xsl:for-each select="rsm:Invoice">
						<span>
							<xsl:text>Stylesheet Lesbarmachung der XML-Daten von ZUGFeRD-Rechnungen</xsl:text>
						</span>
						<br/>
						<span>
							<xsl:text>ZUGFeRD-Version&#160;&#160; : ReleaseCandidate (RC) </xsl:text>
						</span>
						<br/>
						<span>
							<xsl:text>Stylesheet-Version: 1.0 vom 25.06.2014</xsl:text>
						</span>
						<br/>
						<span>
							<xsl:text>Codelisten-Version: 1.0 vom 25.06.2014</xsl:text>
						</span>
						<br/>
						<span>
							<xsl:text>(c) AWV e.V. 2014</xsl:text>
						</span>
						<xsl:variable name="altova:table">
							<table style="border-collapse:collapse; " border="1" width="100%">
								<xsl:variable name="altova:CurrContextGrid_0" select="."/>
								<xsl:variable name="altova:ColumnData"/>
								<tbody>
									<tr>
										<td>
											<br/>
											<h2>
												<xsl:for-each select="rsm:HeaderExchangedDocument">
													<xsl:for-each select="Name">
														<span style="font-weight:bold; ">
															<xsl:apply-templates/>
														</span>
													</xsl:for-each>
													<span style="empty-cells:hide; font-weight:bold; ">
														<xsl:text>(</xsl:text>
													</span>
													<xsl:for-each select="TypeCode">
														<xsl:apply-templates/>
													</xsl:for-each>
													<span style="empty-cells:hide; font-weight:bold; ">
														<xsl:text>) Nr. </xsl:text>
													</span>
													<xsl:for-each select="ID">
														<span style="font-weight:bold; ">
															<xsl:apply-templates/>
														</span>
													</xsl:for-each>
													<span style="empty-cells:hide; font-weight:bold; ">
														<xsl:text> vom </xsl:text>
													</span>
													<xsl:for-each select="IssueDateTime">
														<xsl:call-template name="Datum"/>
													</xsl:for-each>
												</xsl:for-each>
											</h2>
											<xsl:for-each select="rsm:SpecifiedExchangedDocumentContext">
												<br/>
												<xsl:for-each select="TestIndicator">
													<br/>
													<span style="empty-cells:hide; ">
														<xsl:text>Testkennzeichen&#160;&#160;&#160;&#160;&#160; : </xsl:text>
													</span>
													<xsl:apply-templates/>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedExchangedDocumentContext">
												<xsl:for-each select="GuidelineSpecifiedDocumentContextParameter">
													<xsl:for-each select="ID">
														<br/>
														<span style="empty-cells:hide; ">
															<xsl:text>Anwendungsempfehlung : </xsl:text>
														</span>
														<xsl:apply-templates/>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:HeaderExchangedDocument"/>
											<xsl:for-each select="rsm:HeaderExchangedDocument"/>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeSettlement">
													<xsl:for-each select="InvoiceCurrencyCode">
														<br/>
														<span style="empty-cells:hide; ">
															<xsl:text>Währung&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
														</span>
														<xsl:apply-templates/>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<br/>
										</td>
									</tr>
									<tr>
										<td>
											<xsl:for-each select="rsm:HeaderExchangedDocument"/>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeDelivery">
													<xsl:for-each select="ActualDeliverySupplyChainEvent">
														<xsl:for-each select="OccurrenceDateTime">
															<br/>
															<span style="empty-cells:hide; ">
																<xsl:text>Liefer- und Leistungsdatum&#160;&#160;&#160; : </xsl:text>
															</span>
															<xsl:call-template name="Datum"/>
														</xsl:for-each>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeSettlement">
													<xsl:for-each select="BillingSpecifiedPeriod">
														<xsl:for-each select="StartDateTime">
															<br/>
															<span>
																<xsl:text>Abrechnungszeitraum&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
															</span>
															<xsl:call-template name="Datum"/>
														</xsl:for-each>
														<span>
															<xsl:text> bis </xsl:text>
														</span>
														<xsl:for-each select="EndDateTime">
															<xsl:call-template name="Datum"/>
														</xsl:for-each>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<br/>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeAgreement">
													<xsl:for-each select="BuyerReference">
														<br/>
														<span style="empty-cells:hide; ">
															<xsl:text>Referenz des Käufers&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
														</span>
														<xsl:apply-templates/>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeSettlement">
													<xsl:for-each select="PaymentReference">
														<br/>
														<span style="empty-cells:hide; font-weight:bold; ">
															<xsl:text>Zahlungsreferenz&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
														</span>
														<span style="font-weight:bold; ">
															<xsl:apply-templates/>
														</span>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeSettlement">
													<xsl:for-each select="ReceivableSpecifiedTradeAccountingAccount">
														<xsl:for-each select="ID">
															<br/>
															<span>
																<xsl:text>Buchungsreferenz&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
															</span>
															<xsl:apply-templates/>
														</xsl:for-each>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeAgreement"/>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeDelivery"/>
											</xsl:for-each>
											<br/>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeAgreement">
													<xsl:for-each select="BuyerOrderReferencedDocument">
														<xsl:for-each select="ID">
															<br/>
															<span style="empty-cells:hide; ">
																<xsl:text>Bestellung&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
															</span>
															<xsl:apply-templates/>
														</xsl:for-each>
														<span style="empty-cells:hide; ">
															<xsl:text> vom </xsl:text>
														</span>
														<xsl:for-each select="IssueDateTime">
															<xsl:call-template name="Datum"/>
														</xsl:for-each>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeDelivery"/>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeDelivery">
													<xsl:for-each select="DeliveryNoteReferencedDocument">
														<xsl:for-each select="ID">
															<br/>
															<span style="empty-cells:hide; ">
																<xsl:text>Lieferschein&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
															</span>
															<xsl:apply-templates/>
														</xsl:for-each>
														<span style="empty-cells:hide; ">
															<xsl:text> vom </xsl:text>
														</span>
														<xsl:for-each select="IssueDateTime">
															<xsl:call-template name="Datum"/>
														</xsl:for-each>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeAgreement">
													<xsl:for-each select="ContractReferencedDocument">
														<xsl:for-each select="ID">
															<br/>
															<span style="empty-cells:hide; ">
																<xsl:text>Vertrag&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
															</span>
															<xsl:apply-templates/>
														</xsl:for-each>
														<span style="empty-cells:hide; ">
															<xsl:text> vom </xsl:text>
														</span>
														<xsl:for-each select="IssueDateTime">
															<xsl:call-template name="Datum"/>
														</xsl:for-each>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeAgreement">
													<xsl:for-each select="CustomerOrderReferencedDocument">
														<xsl:for-each select="ID">
															<br/>
															<span style="empty-cells:hide; ">
																<xsl:text>Kundenbestellung&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
															</span>
															<xsl:apply-templates/>
														</xsl:for-each>
														<span style="empty-cells:hide; ">
															<xsl:text> vom </xsl:text>
														</span>
														<xsl:for-each select="IssueDateTime">
															<xsl:call-template name="Datum"/>
														</xsl:for-each>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeAgreement"/>
											</xsl:for-each>
											<br/>
											<span style="empty-cells:hide; ">
												<xsl:text>&#160;</xsl:text>
											</span>
										</td>
									</tr>
									<tr>
										<td>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeAgreement">
													<xsl:for-each select="SellerTradeParty">
														<br/>
														<span style="empty-cells:hide; font-weight:bold; text-decoration:underline; ">
															<xsl:text>Verkäufer:</xsl:text>
														</span>
														<br/>
														<xsl:call-template name="Party"/>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeAgreement">
													<xsl:for-each select="BuyerTradeParty">
														<br/>
														<span style="empty-cells:hide; font-weight:bold; text-decoration:underline; ">
															<xsl:text>Käufer:</xsl:text>
														</span>
														<br/>
														<xsl:call-template name="Party"/>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeAgreement"/>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeDelivery"/>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeDelivery"/>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeDelivery"/>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeSettlement">
													<xsl:for-each select="InvoiceeTradeParty">
														<br/>
														<span style="empty-cells:hide; font-weight:bold; text-decoration:underline; ">
															<xsl:text>Abweichender Rechnungsempfänger:</xsl:text>
														</span>
														<br/>
														<xsl:call-template name="Party"/>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeSettlement"/>
											</xsl:for-each>
										</td>
									</tr>
									<tr>
										<td>
											<xsl:for-each select="rsm:HeaderExchangedDocument">
												<br/>
												<xsl:for-each select="IncludedNote">
													<xsl:for-each select="SubjectCode">
														<span style="color:gray; empty-cells:hide; ">
															<xsl:text>Qualifizierung der Textart: </xsl:text>
														</span>
														<span style="font-weight:bold; ">
															<xsl:apply-templates/>
														</span>
													</xsl:for-each>
													<xsl:for-each select="Content">
														<br/>
														<span style="font-weight:bold; white-space:pre-wrap; ">
															<xsl:apply-templates/>
														</span>
														<br/>
														<br/>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
										</td>
									</tr>
									<tr>
										<td style="border-bottom-style:none; ">
											<xsl:variable name="altova:table">
												<table style="border:0px; border-collapse:collapse; border-spacing:0px; padding:0px; width:100%; " border="1">
													<xsl:variable name="altova:CurrContextGrid_1" select="."/>
													<xsl:variable name="altova:ColumnData"/>
													<tbody>
														<tr style="text-align:center; vertical-align:middle; ">
															<td style="border-bottom-style:solid; border-collapse:collapse; border-left-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:center; width:30px; ">
																<span style="font-weight:bold; ">
																	<xsl:text>Pos</xsl:text>
																</span>
															</td>
															<td style="border-bottom-style:solid; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:left; width:110px; ">
																<span style="font-weight:bold; ">
																	<xsl:text>Art-Nr-Kunde</xsl:text>
																</span>
																<br/>
																<span style="font-weight:bold; ">
																	<xsl:text>Art-Nr-Lief.</xsl:text>
																</span>
																<br/>
																<span style="font-weight:bold; ">
																	<xsl:text>Art-Nr</xsl:text>
																</span>
																<br/>
																<span style="font-weight:bold; ">
																	<xsl:text>(Art)</xsl:text>
																</span>
															</td>
															<td style="border-bottom-style:solid; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; ">
																<span style="font-weight:bold; ">
																	<xsl:text>Beschreibung</xsl:text>
																</span>
															</td>
															<td style="border-bottom-style:solid; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:110px; ">
																<span style="font-weight:bold; ">
																	<xsl:text>E-Preis</xsl:text>
																</span>
															</td>
															<td style="border-bottom-style:solid; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:120px; ">
																<span style="font-weight:bold; ">
																	<xsl:text>Menge</xsl:text>
																</span>
															</td>
															<td style="border-bottom-style:solid; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:110px; ">
																<span style="font-weight:bold; ">
																	<xsl:text>Steuer</xsl:text>
																</span>
															</td>
															<td style="border-bottom-style:solid; border-collapse:collapse; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:110px; ">
																<span style="font-weight:bold; ">
																	<xsl:text>Entgelt</xsl:text>
																</span>
															</td>
														</tr>
													</tbody>
												</table>
											</xsl:variable>
											<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
											<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
											<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
												<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
											</xsl:apply-templates>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="IncludedSupplyChainTradeLineItem">
													<xsl:if test="fn:not( fn:empty(SpecifiedTradeProduct))">
														<xsl:variable name="altova:table">
															<table style="border:0px; border-collapse:collapse; border-spacing:0px; padding:0px; width:100%; " border="1">
																<xsl:variable name="altova:CurrContextGrid_2" select="."/>
																<xsl:variable name="altova:ColumnData"/>
																<tbody>
																	<tr style="vertical-align:top; ">
																		<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:center; width:30px; ">
																			<xsl:for-each select="AssociatedDocumentLineDocument">
																				<xsl:if test="fn:empty(IncludedNote)">
																					<xsl:for-each select="LineID">
																						<xsl:apply-templates/>
																					</xsl:for-each>
																				</xsl:if>
																			</xsl:for-each>
																		</td>
																		<td style="border-bottom-style:none; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; width:110px; ">
																			<xsl:for-each select="SpecifiedTradeProduct">
																				<xsl:for-each select="BuyerAssignedID">
																					<xsl:apply-templates/>
																				</xsl:for-each>
																				<br/>
																				<xsl:for-each select="SellerAssignedID">
																					<xsl:apply-templates/>
																				</xsl:for-each>
																				<br/>
																				<xsl:for-each select="GlobalID">
																					<xsl:apply-templates/>
																					<br/>
																					<xsl:for-each select="@schemeID">
																						<xsl:call-template name="Global_Identifier"/>
																					</xsl:for-each>
																				</xsl:for-each>
																			</xsl:for-each>
																		</td>
																		<td style="border-bottom-style:none; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; ">
																			<xsl:for-each select="SpecifiedTradeProduct">
																				<xsl:for-each select="Name">
																					<span style="white-space:pre-wrap; ">
																						<xsl:apply-templates/>
																					</span>
																					<br/>
																				</xsl:for-each>
																			</xsl:for-each>
																			<xsl:for-each select="SpecifiedSupplyChainTradeAgreement">
																				<xsl:if test="fn:exists(GrossPriceProductTradePrice)">
																					<xsl:for-each select="GrossPriceProductTradePrice">
																						<xsl:for-each select="BasisQuantity">
																							<span>
																								<xsl:text>Preisbasismenge: </xsl:text>
																							</span>
																							<xsl:call-template name="Menge"/>
																						</xsl:for-each>
																					</xsl:for-each>
																				</xsl:if>
																				<xsl:if test="fn:not(fn:exists(GrossPriceProductTradePrice))">
																					<xsl:for-each select="NetPriceProductTradePrice">
																						<xsl:for-each select="BasisQuantity">
																							<br/>
																							<span>
																								<xsl:text>Preisbasismenge: </xsl:text>
																							</span>
																							<xsl:call-template name="Menge"/>
																						</xsl:for-each>
																					</xsl:for-each>
																				</xsl:if>
																			</xsl:for-each>
																		</td>
																		<td style="border-bottom-style:solid; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:110px; ">
																			<xsl:for-each select="SpecifiedSupplyChainTradeAgreement">
																				<xsl:if test="fn:exists(GrossPriceProductTradePrice)">
																					<xsl:for-each select="GrossPriceProductTradePrice">
																						<xsl:for-each select="ChargeAmount">
																							<span>
																								<xsl:variable name="altova:seqContentStrings_3">
																									<xsl:value-of select="format-number(number(string(.)), '##0,0000', 'format1')"/>
																								</xsl:variable>
																								<xsl:variable name="altova:sContent_3" select="string($altova:seqContentStrings_3)"/>
																								<xsl:value-of select="$altova:sContent_3"/>
																							</span>
																						</xsl:for-each>
																					</xsl:for-each>
																				</xsl:if>
																				<xsl:if test="not(fn:exists(GrossPriceProductTradePrice))">
																					<xsl:for-each select="NetPriceProductTradePrice">
																						<xsl:for-each select="ChargeAmount">
																							<span>
																								<xsl:variable name="altova:seqContentStrings_4">
																									<xsl:value-of select="format-number(number(string(.)), '###.##0,00', 'format1')"/>
																								</xsl:variable>
																								<xsl:variable name="altova:sContent_4" select="string($altova:seqContentStrings_4)"/>
																								<xsl:value-of select="$altova:sContent_4"/>
																							</span>
																						</xsl:for-each>
																					</xsl:for-each>
																				</xsl:if>
																			</xsl:for-each>
																		</td>
																		<td style="border-bottom-style:solid; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:120px; ">
																			<xsl:for-each select="SpecifiedSupplyChainTradeDelivery">
																				<xsl:for-each select="BilledQuantity">
																					<xsl:call-template name="Menge"/>
																				</xsl:for-each>
																			</xsl:for-each>
																		</td>
																		<td style="border-bottom-style:solid; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:110px; ">
																			<xsl:for-each select="SpecifiedSupplyChainTradeSettlement">
																				<xsl:for-each select="ApplicableTradeTax">
																					<xsl:call-template name="TradeTax"/>
																				</xsl:for-each>
																			</xsl:for-each>
																		</td>
																		<td style="border-bottom-style:solid; border-collapse:collapse; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:110px; ">
																			<xsl:for-each select="SpecifiedSupplyChainTradeSettlement">
																				<xsl:for-each select="SpecifiedTradeSettlementMonetarySummation">
																					<xsl:for-each select="LineTotalAmount">
																						<span>
																							<xsl:variable name="altova:seqContentStrings_5">
																								<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																							</xsl:variable>
																							<xsl:variable name="altova:sContent_5" select="string($altova:seqContentStrings_5)"/>
																							<xsl:value-of select="$altova:sContent_5"/>
																						</span>
																					</xsl:for-each>
																				</xsl:for-each>
																			</xsl:for-each>
																		</td>
																	</tr>
																</tbody>
															</table>
														</xsl:variable>
														<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
														<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
														<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
															<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
														</xsl:apply-templates>
													</xsl:if>
													<xsl:for-each select="AssociatedDocumentLineDocument">
														<xsl:if test="fn:not( fn:empty(IncludedNote))">
															<xsl:variable name="altova:table">
																<table style="border:0px; border-collapse:collapse; border-spacing:0px; border-top-style:none; padding:0px; width:100%; " border="1">
																	<xsl:variable name="altova:CurrContextGrid_6" select="."/>
																	<xsl:variable name="altova:ColumnData"/>
																	<tbody>
																		<tr style="empty-cells:hide; vertical-align:top; ">
																			<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:center; width:30px; ">
																				<xsl:for-each select="LineID">
																					<xsl:apply-templates/>
																				</xsl:for-each>
																			</td>
																			<td style="border-bottom-style:none; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; width:110px; "/>
																			<td style="border-bottom-style:none; border-collapse:collapse; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; ">
																				<xsl:for-each select="IncludedNote">
																					<xsl:for-each select="Content">
																						<span style="white-space:pre-wrap; ">
																							<xsl:apply-templates/>
																						</span>
																					</xsl:for-each>
																					<xsl:for-each select="SubjectCode">
																						<br/>
																						<span style="color:gray; empty-cells:hide; ">
																							<xsl:text>Qualifizierung der Textart: </xsl:text>
																						</span>
																						<xsl:apply-templates/>
																					</xsl:for-each>
																					<br/>
																				</xsl:for-each>
																			</td>
																			<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; width:110px; "/>
																		</tr>
																	</tbody>
																</table>
															</xsl:variable>
															<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
															<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
															<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
																<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
															</xsl:apply-templates>
														</xsl:if>
													</xsl:for-each>
													<xsl:for-each select="SpecifiedSupplyChainTradeAgreement">
														<xsl:for-each select="GrossPriceProductTradePrice">
															<xsl:if test="fn:not( fn:empty(AppliedTradeAllowanceCharge))">
																<xsl:variable name="altova:table">
																	<table style="border:0px; border-collapse:collapse; border-spacing:0px; padding:0px; width:100%; " border="1">
																		<xsl:variable name="altova:CurrContextGrid_7" select="."/>
																		<xsl:variable name="altova:ColumnData"/>
																		<tbody>
																			<tr style="empty-cells:hide; vertical-align:top; ">
																				<td style="border-bottom-style:none; border-left-style:none; border-top-style:none; width:30px; "/>
																				<td style="border-bottom-style:none; border-top-style:none; width:110px; "/>
																				<td style="border-bottom-style:none; border-right-style:none; border-top-style:none; ">
																					<xsl:variable name="altova:table">
																						<table style="border:0px; border-style:none; padding:0px; width:100%; " border="1">
																							<xsl:variable name="altova:CurrContextGrid_8" select="."/>
																							<xsl:variable name="altova:ColumnData"/>
																							<thead>
																								<tr>
																									<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; font-weight:normal; text-align:left; width:70px; ">
																										<span style="font-style:italic; ">
																											<xsl:text>Art</xsl:text>
																										</span>
																									</th>
																									<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; font-weight:normal; text-align:left; ">
																										<span style="font-style:italic; ">
																											<xsl:text>Grund</xsl:text>
																										</span>
																									</th>
																									<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; font-weight:normal; text-align:right; width:70px; ">
																										<span style="font-style:italic; ">
																											<xsl:text>%</xsl:text>
																										</span>
																									</th>
																									<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; font-weight:normal; text-align:right; width:110px; ">
																										<span style="font-style:italic; ">
																											<xsl:text>Basisbetrag</xsl:text>
																										</span>
																									</th>
																									<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; font-weight:normal; text-align:right; width:120px; ">
																										<span style="font-style:italic; ">
																											<xsl:text>Basismenge</xsl:text>
																										</span>
																									</th>
																									<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; font-weight:normal; text-align:right; width:110px; ">
																										<span style="font-style:italic; ">
																											<xsl:text>Betrag</xsl:text>
																										</span>
																									</th>
																									<th style="border-bottom-style:none; border-left-style:none; border-right-style:none; border-top-style:none; width:112px; "/>
																								</tr>
																							</thead>
																							<tbody>
																								<xsl:for-each select="AppliedTradeAllowanceCharge">
																									<xsl:sort select="SequenceNumeric" data-type="text" order="ascending"/>
																									<tr>
																										<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; vertical-align:top; width:70px; ">
																											<xsl:for-each select="ChargeIndicator">
																												<span>
																													<xsl:value-of select="if (. eq fn:true())then &apos;Zuschlag&apos; else &apos;Abschlag&apos;"/>
																												</span>
																											</xsl:for-each>
																										</td>
																										<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; vertical-align:top; ">
																											<xsl:for-each select="Reason">
																												<xsl:apply-templates/>
																											</xsl:for-each>
																										</td>
																										<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; vertical-align:top; width:70px; "/>
																										<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; vertical-align:top; width:110px; ">
																											<xsl:for-each select="BasisAmount">
																												<span>
																													<xsl:variable name="altova:seqContentStrings_9">
																														<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																													</xsl:variable>
																													<xsl:variable name="altova:sContent_9" select="string($altova:seqContentStrings_9)"/>
																													<xsl:value-of select="$altova:sContent_9"/>
																												</span>
																											</xsl:for-each>
																										</td>
																										<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; vertical-align:top; width:120px; "/>
																										<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; vertical-align:top; width:110px; ">
																											<xsl:for-each select="ActualAmount">
																												<span>
																													<xsl:variable name="altova:seqContentStrings_10">
																														<xsl:value-of select="format-number(number(if(../ChargeIndicator = fn:false()) then -1*. else .), '##0,00', 'format1')"/>
																													</xsl:variable>
																													<xsl:variable name="altova:sContent_10" select="string($altova:seqContentStrings_10)"/>
																													<xsl:value-of select="$altova:sContent_10"/>
																												</span>
																											</xsl:for-each>
																										</td>
																										<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:108px; "/>
																									</tr>
																								</xsl:for-each>
																							</tbody>
																						</table>
																					</xsl:variable>
																					<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
																					<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
																					<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
																						<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
																					</xsl:apply-templates>
																				</td>
																			</tr>
																		</tbody>
																	</table>
																</xsl:variable>
																<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
																<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
																<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
																	<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
																</xsl:apply-templates>
															</xsl:if>
															<xsl:if test="fn:exists(../../SpecifiedSupplyChainTradeSettlement/SpecifiedTradeSettlementMonetarySummation/TotalAllowanceChargeAmount)">
																<xsl:variable name="altova:table">
																	<table style="border:0px; border-collapse:collapse; border-spacing:0px; padding:0px; width:100%; " border="1">
																		<xsl:variable name="altova:CurrContextGrid_11" select="."/>
																		<xsl:variable name="altova:ColumnData"/>
																		<tbody>
																			<tr style="empty-cells:hide; vertical-align:top; ">
																				<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; width:30px; "/>
																				<td style="border-bottom-style:none; border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; width:110px; "/>
																				<td style="border-bottom-style:none; border-collapse:collapse; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; ">
																					<br/>
																				</td>
																				<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:110px; ">
																					<span>
																						<xsl:text>Summe:</xsl:text>
																					</span>
																				</td>
																				<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:double; empty-cells:hide; text-align:right; width:110px; ">
																					<xsl:for-each select="$XML">
																						<xsl:for-each select="rsm:Invoice">
																							<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
																								<xsl:for-each select="IncludedSupplyChainTradeLineItem">
																									<xsl:for-each select="SpecifiedSupplyChainTradeSettlement">
																										<xsl:for-each select="SpecifiedTradeSettlementMonetarySummation"/>
																									</xsl:for-each>
																								</xsl:for-each>
																							</xsl:for-each>
																						</xsl:for-each>
																					</xsl:for-each>
																				</td>
																				<td style="border-bottom-style:none; border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; text-align:right; width:110px; "/>
																			</tr>
																		</tbody>
																	</table>
																</xsl:variable>
																<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
																<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
																<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
																	<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
																</xsl:apply-templates>
															</xsl:if>
														</xsl:for-each>
													</xsl:for-each>
													<xsl:variable name="altova:table">
														<table style="border:0px; border-collapse:collapse; border-spacing:0px; border-top-style:none; padding:0px; width:100%; " border="1">
															<xsl:variable name="altova:CurrContextGrid_12" select="."/>
															<xsl:variable name="altova:ColumnData"/>
															<tbody>
																<tr style="empty-cells:hide; vertical-align:top; ">
																	<td style="border-collapse:collapse; border-left-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; width:30px; "/>
																	<td style="border-collapse:collapse; border-spacing:0px; border-top-style:none; empty-cells:hide; width:110px; "/>
																	<td style="border-collapse:collapse; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; ">
																		<xsl:for-each select="SpecifiedTradeProduct">
																			<xsl:for-each select="Description">
																				<br/>
																				<span style="white-space:pre-wrap; ">
																					<xsl:apply-templates/>
																				</span>
																			</xsl:for-each>
																		</xsl:for-each>
																		<xsl:for-each select="SpecifiedSupplyChainTradeDelivery">
																			<xsl:for-each select="ActualDeliverySupplyChainEvent">
																				<xsl:for-each select="OccurrenceDateTime">
																					<br/>
																					<span>
																						<xsl:text>* Leistungsdatum: </xsl:text>
																					</span>
																					<xsl:call-template name="Datum"/>
																				</xsl:for-each>
																			</xsl:for-each>
																		</xsl:for-each>
																		<xsl:for-each select="SpecifiedSupplyChainTradeSettlement">
																			<xsl:for-each select="BillingSpecifiedPeriod">
																				<br/>
																				<span>
																					<xsl:text>* Berechnungszeitraum: </xsl:text>
																				</span>
																				<xsl:for-each select="StartDateTime">
																					<xsl:call-template name="Datum"/>
																				</xsl:for-each>
																				<xsl:for-each select="EndDateTime">
																					<span>
																						<xsl:text> bis </xsl:text>
																					</span>
																					<xsl:call-template name="Datum"/>
																				</xsl:for-each>
																			</xsl:for-each>
																		</xsl:for-each>
																		<xsl:for-each select="SpecifiedTradeProduct">
																			<xsl:for-each select="OriginTradeCountry">
																				<xsl:for-each select="ID">
																					<br/>
																					<span>
																						<xsl:text>* Herkunftsland: </xsl:text>
																					</span>
																					<xsl:apply-templates/>
																				</xsl:for-each>
																			</xsl:for-each>
																		</xsl:for-each>
																		<xsl:for-each select="SpecifiedSupplyChainTradeDelivery">
																			<xsl:call-template name="PackageQuantity"/>
																		</xsl:for-each>
																		<xsl:for-each select="SpecifiedSupplyChainTradeAgreement">
																			<xsl:for-each select="BuyerOrderReferencedDocument">
																				<br/>
																				<span>
																					<xsl:text>* Bestellung</xsl:text>
																				</span>
																				<xsl:call-template name="Positionsreferenz"/>
																			</xsl:for-each>
																			<xsl:for-each select="ContractReferencedDocument">
																				<br/>
																				<span>
																					<xsl:text>* Vertrag</xsl:text>
																				</span>
																				<xsl:call-template name="Positionsreferenz"/>
																			</xsl:for-each>
																			<xsl:for-each select="CustomerOrderReferencedDocument">
																				<br/>
																				<span>
																					<xsl:text>* Kundenbestellung</xsl:text>
																				</span>
																				<xsl:call-template name="Positionsreferenz"/>
																			</xsl:for-each>
																		</xsl:for-each>
																		<xsl:for-each select="SpecifiedSupplyChainTradeDelivery">
																			<xsl:for-each select="DeliveryNoteReferencedDocument">
																				<br/>
																				<span>
																					<xsl:text>* Lieferschein</xsl:text>
																				</span>
																				<xsl:call-template name="Positionsreferenz"/>
																			</xsl:for-each>
																		</xsl:for-each>
																		<xsl:for-each select="SpecifiedSupplyChainTradeSettlement"/>
																		<xsl:for-each select="SpecifiedTradeProduct"/>
																		<xsl:for-each select="SpecifiedSupplyChainTradeDelivery"/>
																		<xsl:for-each select="SpecifiedTradeProduct">
																			<xsl:if test="fn:exists(ApplicableProductCharacteristic)">
																				<br/>
																				<br/>
																				<span style="text-decoration:underline; ">
																					<xsl:text>Weitere Eigenschaften:</xsl:text>
																				</span>
																				<br/>
																			</xsl:if>
																		</xsl:for-each>
																		<xsl:if test="fn:exists(SpecifiedTradeProduct/IncludedReferencedProduct)">
																			<br/>
																			<br/>
																			<span style="text-decoration:underline; ">
																				<xsl:text>Basisartikel:</xsl:text>
																			</span>
																			<br/>
																		</xsl:if>
																		<xsl:for-each select="SpecifiedTradeProduct">
																			<xsl:if test="fn:exists(IncludedReferencedProduct)">
																				<xsl:variable name="altova:table">
																					<table style="border:0px; width:100%; " border="1">
																						<xsl:variable name="altova:CurrContextGrid_13" select="."/>
																						<xsl:variable name="altova:ColumnData"/>
																						<thead>
																							<tr>
																								<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; text-align:left; width:110px; ">
																									<span style="font-weight:bold; ">
																										<xsl:text>Art-Nr-Kunde</xsl:text>
																									</span>
																									<br/>
																								</th>
																								<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; width:110px; ">
																									<span style="font-weight:bold; ">
																										<xsl:text>Art-Nr-Lief.</xsl:text>
																									</span>
																								</th>
																								<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; width:110px; ">
																									<span>
																										<xsl:text>Art-Nr. (Art)</xsl:text>
																									</span>
																								</th>
																								<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; ">
																									<span>
																										<xsl:text>Beschreibung</xsl:text>
																									</span>
																								</th>
																								<th style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; text-align:right; width:120px; ">
																									<span>
																										<xsl:text>Menge</xsl:text>
																									</span>
																								</th>
																							</tr>
																						</thead>
																						<tbody/>
																					</table>
																				</xsl:variable>
																				<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
																				<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
																				<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
																					<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
																				</xsl:apply-templates>
																			</xsl:if>
																		</xsl:for-each>
																	</td>
																	<td style="border-collapse:collapse; border-left-style:none; border-right-style:none; border-spacing:0px; border-top-style:none; empty-cells:hide; width:110px; "/>
																</tr>
															</tbody>
														</table>
													</xsl:variable>
													<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
													<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
													<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
														<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
													</xsl:apply-templates>
												</xsl:for-each>
											</xsl:for-each>
											<br/>
										</td>
									</tr>
									<tr>
										<td style="border-bottom-style:none; border-top-style:none; ">
											<xsl:if test="fn:exists(rsm:SpecifiedSupplyChainTradeTransaction/ApplicableSupplyChainTradeSettlement/SpecifiedTradePaymentTerms)">
												<br/>
												<span style="empty-cells:hide; font-weight:bold; text-decoration:underline; ">
													<xsl:text>Zahlungsbedingungen:</xsl:text>
												</span>
												<br/>
												<br/>
												<xsl:variable name="altova:table">
													<table style="border:0px; border-collapse:collapse; border-right-style:none; width:100%; " border="1">
														<xsl:variable name="altova:CurrContextGrid_14" select="."/>
														<xsl:variable name="altova:ColumnData"/>
														<thead>
															<tr>
																<th style="border-left-style:none; ">
																	<span>
																		<xsl:text>Beschreibung</xsl:text>
																	</span>
																</th>
																<th style="width:110px; ">
																	<span>
																		<xsl:text>Fälligkeit</xsl:text>
																	</span>
																</th>
																<th style="border-right-style:none; width:110px; ">
																	<span>
																		<xsl:text>Teilzahlung</xsl:text>
																	</span>
																</th>
															</tr>
														</thead>
														<tbody>
															<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
																<xsl:for-each select="ApplicableSupplyChainTradeSettlement">
																	<xsl:for-each select="SpecifiedTradePaymentTerms">
																		<tr>
																			<td style="border-bottom-style:none; border-left-style:none; ">
																				<xsl:for-each select="Description">
																					<span style="white-space:pre-wrap; ">
																						<xsl:apply-templates/>
																					</span>
																				</xsl:for-each>
																			</td>
																			<td style="border-bottom-style:none; text-align:center; width:110px; ">
																				<xsl:for-each select="DueDateDateTime">
																					<xsl:call-template name="Datum"/>
																				</xsl:for-each>
																			</td>
																			<td style="border-bottom-style:none; border-right-style:none; text-align:right; width:110px; "/>
																		</tr>
																		<tr>
																			<td colspan="3" style="border-left-style:none; border-right-style:none; border-top-style:none; min-height:0px; ">
																				<xsl:if test="fn:exists(ApplicableTradePaymentPenaltyTerms) or fn:exists(ApplicableTradePaymentDiscountTerms)">
																					<xsl:variable name="altova:table">
																						<table style="border:0px; border-collapse:collapse; border-left-style:none; border-top-style:none; margin:0px; padding:0px; " border="1" width="100%">
																							<xsl:variable name="altova:CurrContextGrid_15" select="."/>
																							<xsl:variable name="altova:ColumnData"/>
																							<tbody>
																								<tr style="border-bottom-style:none; border-top-style:none; ">
																									<td style="border-bottom-style:none; border-left-style:none; border-top-style:none; min-height:0px; "/>
																									<td style="border-bottom-style:none; min-height:0px; text-align:center; width:110px; ">
																										<span style="font-weight:bold; ">
																											<xsl:text>Bezug</xsl:text>
																										</span>
																									</td>
																									<td style="border-bottom-style:none; min-height:0px; text-align:center; width:110px; ">
																										<span style="font-weight:bold; ">
																											<xsl:text>Innerhalb</xsl:text>
																										</span>
																									</td>
																									<td style="border-bottom-style:none; min-height:0px; text-align:center; width:110px; ">
																										<span style="font-weight:bold; ">
																											<xsl:text>Basisbetrag</xsl:text>
																										</span>
																									</td>
																									<td style="border-bottom-style:none; min-height:0px; text-align:center; width:110px; ">
																										<span style="font-weight:bold; ">
																											<xsl:text>%</xsl:text>
																										</span>
																									</td>
																									<td style="border-bottom-style:none; border-right-style:none; min-height:0px; text-align:center; width:108px; ">
																										<span style="font-weight:bold; ">
																											<xsl:text>Betrag</xsl:text>
																										</span>
																									</td>
																								</tr>
																							</tbody>
																						</table>
																					</xsl:variable>
																					<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
																					<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
																					<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
																						<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
																					</xsl:apply-templates>
																				</xsl:if>
																				<xsl:if test="fn:exists(ApplicableTradePaymentPenaltyTerms)">
																					<xsl:variable name="altova:table">
																						<table style="border:0px; border-bottom-style:none; border-collapse:collapse; margin:0px; padding:0px; width:100%; " border="1">
																							<xsl:variable name="altova:CurrContextGrid_16" select="."/>
																							<xsl:variable name="altova:ColumnData"/>
																							<tbody/>
																						</table>
																					</xsl:variable>
																					<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
																					<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
																					<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
																						<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
																					</xsl:apply-templates>
																				</xsl:if>
																				<xsl:if test="fn:exists(ApplicableTradePaymentDiscountTerms)">
																					<xsl:variable name="altova:table">
																						<table style="border:0px; border-collapse:collapse; margin:0px; padding:0px; width:100%; " border="1">
																							<xsl:variable name="altova:CurrContextGrid_17" select="."/>
																							<xsl:variable name="altova:ColumnData"/>
																							<tbody/>
																						</table>
																					</xsl:variable>
																					<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
																					<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
																					<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
																						<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
																					</xsl:apply-templates>
																				</xsl:if>
																			</td>
																		</tr>
																	</xsl:for-each>
																</xsl:for-each>
															</xsl:for-each>
														</tbody>
													</table>
												</xsl:variable>
												<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
												<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
												<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
													<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
												</xsl:apply-templates>
											</xsl:if>
											<xsl:if test="fn:exists(rsm:SpecifiedSupplyChainTradeTransaction/ApplicableSupplyChainTradeSettlement/SpecifiedTradeSettlementPaymentMeans)">
												<br/>
												<span style="font-weight:bold; text-decoration:underline; ">
													<xsl:text>Zahlungsart:</xsl:text>
												</span>
												<br/>
												<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
													<xsl:for-each select="ApplicableSupplyChainTradeSettlement">
														<xsl:for-each select="SpecifiedTradeSettlementPaymentMeans">
															<xsl:for-each select="Information">
																<br/>
																<xsl:apply-templates/>
															</xsl:for-each>
															<xsl:for-each select="TypeCode">
																<br/>
																<span style="empty-cells:hide; ">
																	<xsl:text>Zahlungstyp (codiert): </xsl:text>
																</span>
																<xsl:call-template name="PaymentType"/>
															</xsl:for-each>
															<xsl:for-each select="PayerPartyDebtorFinancialAccount">
																<xsl:for-each select="IBANID">
																	<br/>
																	<span style="empty-cells:hide; ">
																		<xsl:text>Käufer-IBAN&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
																	</span>
																	<xsl:apply-templates/>
																</xsl:for-each>
																<xsl:for-each select="ProprietaryID">
																	<br/>
																	<span style="empty-cells:hide; ">
																		<xsl:text>Käufer-Kontonummer&#160;&#160; : </xsl:text>
																	</span>
																	<xsl:apply-templates/>
																</xsl:for-each>
															</xsl:for-each>
															<xsl:for-each select="PayerSpecifiedDebtorFinancialInstitution">
																<xsl:for-each select="BICID">
																	<br/>
																	<span style="empty-cells:hide; ">
																		<xsl:text>Käufer-BIC&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
																	</span>
																	<xsl:apply-templates/>
																</xsl:for-each>
																<xsl:for-each select="GermanBankleitzahlID">
																	<br/>
																	<span style="empty-cells:hide; ">
																		<xsl:text>Käufer-Bankleitzahl&#160; : </xsl:text>
																	</span>
																	<xsl:apply-templates/>
																</xsl:for-each>
																<xsl:for-each select="Name">
																	<br/>
																	<span style="empty-cells:hide; ">
																		<xsl:text>Käufer-Kreditinstitut: </xsl:text>
																	</span>
																	<xsl:apply-templates/>
																</xsl:for-each>
															</xsl:for-each>
															<br/>
															<xsl:for-each select="PayeePartyCreditorFinancialAccount">
																<xsl:for-each select="IBANID">
																	<br/>
																	<span style="empty-cells:hide; ">
																		<xsl:text>Verkäufer-IBAN&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
																	</span>
																	<xsl:apply-templates/>
																</xsl:for-each>
																<xsl:for-each select="ProprietaryID">
																	<br/>
																	<span style="empty-cells:hide; ">
																		<xsl:text>Verkäufer-Kontonummer: </xsl:text>
																	</span>
																	<xsl:apply-templates/>
																</xsl:for-each>
															</xsl:for-each>
															<xsl:for-each select="PayeeSpecifiedCreditorFinancialInstitution">
																<xsl:for-each select="BICID">
																	<br/>
																	<span style="empty-cells:hide; ">
																		<xsl:text>Verkäufer-BIC&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
																	</span>
																	<xsl:apply-templates/>
																</xsl:for-each>
																<xsl:for-each select="GermanBankleitzahlID">
																	<br/>
																	<span style="empty-cells:hide; ">
																		<xsl:text>Verkäufer-BLZ&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
																	</span>
																	<xsl:apply-templates/>
																</xsl:for-each>
																<xsl:for-each select="Name">
																	<br/>
																	<span style="empty-cells:hide; ">
																		<xsl:text>Verkäufer-Kreditinst.: </xsl:text>
																	</span>
																	<xsl:apply-templates/>
																</xsl:for-each>
															</xsl:for-each>
														</xsl:for-each>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:if>
										</td>
									</tr>
									<tr>
										<td style="border-bottom-style:none; border-right-style:none; border-top-style:none; ">
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeSettlement">
													<br/>
													<xsl:if test="fn:exists(SpecifiedTradeAllowanceCharge) or fn:exists(SpecifiedLogisticsServiceCharge)">
														<span style="font-weight:bold; text-decoration:underline; ">
															<xsl:text>Zu- und Abschläge:</xsl:text>
														</span>
														<br/>
														<br/>
														<xsl:variable name="altova:table">
															<table style="border:0px; border-collapse:collapse; empty-cells:hide; width:100%; " border="1">
																<xsl:variable name="altova:CurrContextGrid_18" select="."/>
																<xsl:variable name="altova:ColumnData"/>
																<thead>
																	<tr>
																		<th colspan="2" style="border-left-style:none; ">
																			<span>
																				<xsl:text>Art</xsl:text>
																			</span>
																		</th>
																		<th>
																			<span>
																				<xsl:text>%</xsl:text>
																			</span>
																		</th>
																		<th>
																			<span>
																				<xsl:text>Basisbetrag</xsl:text>
																			</span>
																		</th>
																		<th style="width:110px; ">
																			<span>
																				<xsl:text>Basismenge</xsl:text>
																			</span>
																		</th>
																		<th style="width:110px; ">
																			<span>
																				<xsl:text>Steuer</xsl:text>
																			</span>
																		</th>
																		<th style="border-right-style:none; width:110px; ">
																			<span>
																				<xsl:text>Betrag</xsl:text>
																			</span>
																		</th>
																	</tr>
																</thead>
																<tbody>
																	<xsl:for-each select="SpecifiedTradeAllowanceCharge">
																		<xsl:sort select="SequenceNumeric" data-type="text" order="ascending"/>
																		<tr>
																			<td style="border-left-style:none; ">
																				<xsl:for-each select="ChargeIndicator">
																					<span>
																						<xsl:value-of select="if (. eq fn:true())then &apos;Zuschlag&apos; else &apos;Abschlag&apos;"/>
																					</span>
																				</xsl:for-each>
																			</td>
																			<td>
																				<xsl:for-each select="Reason">
																					<xsl:apply-templates/>
																				</xsl:for-each>
																			</td>
																			<td style="text-align:right; "/>
																			<td style="text-align:right; ">
																				<xsl:for-each select="BasisAmount">
																					<span>
																						<xsl:variable name="altova:seqContentStrings_19">
																							<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																						</xsl:variable>
																						<xsl:variable name="altova:sContent_19" select="string($altova:seqContentStrings_19)"/>
																						<xsl:value-of select="$altova:sContent_19"/>
																					</span>
																				</xsl:for-each>
																			</td>
																			<td style="text-align:right; width:110px; "/>
																			<td style="text-align:right; width:110px; ">
																				<xsl:for-each select="CategoryTradeTax">
																					<xsl:call-template name="TradeTax"/>
																				</xsl:for-each>
																			</td>
																			<td style="border-right-style:none; text-align:right; width:110px; ">
																				<xsl:for-each select="ActualAmount">
																					<span>
																						<xsl:variable name="altova:seqContentStrings_20">
																							<xsl:value-of select="format-number(number(if (../ChargeIndicator eq fn:true()) then . else -1 * .), '##0,00', 'format1')"/>
																						</xsl:variable>
																						<xsl:variable name="altova:sContent_20" select="string($altova:seqContentStrings_20)"/>
																						<xsl:value-of select="$altova:sContent_20"/>
																					</span>
																				</xsl:for-each>
																			</td>
																		</tr>
																	</xsl:for-each>
																</tbody>
															</table>
														</xsl:variable>
														<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
														<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
														<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
															<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
														</xsl:apply-templates>
														<xsl:if test="fn:exists(SpecifiedLogisticsServiceCharge)">
															<xsl:if test="fn:not(fn:exists(SpecifiedTradeAllowanceCharge))">
																<xsl:variable name="altova:table">
																	<table style="border:0px; border-top-style:none; width:100%; " border="1">
																		<xsl:variable name="altova:CurrContextGrid_21" select="."/>
																		<xsl:variable name="altova:ColumnData"/>
																		<tbody>
																			<xsl:for-each select="SpecifiedLogisticsServiceCharge">
																				<tr>
																					<td style="border-left-style:none; border-top-style:solid; text-align:center; ">
																						<span style="font-weight:bold; ">
																							<xsl:text>Art</xsl:text>
																						</span>
																					</td>
																					<td style="border-top-style:solid; text-align:center; width:110px; ">
																						<span style="font-weight:bold; ">
																							<xsl:text>Steuer</xsl:text>
																						</span>
																					</td>
																					<td style="border-right-style:none; border-top-style:solid; text-align:center; width:110px; ">
																						<span style="font-weight:bold; ">
																							<xsl:text>Betrag</xsl:text>
																						</span>
																					</td>
																				</tr>
																			</xsl:for-each>
																		</tbody>
																	</table>
																</xsl:variable>
																<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
																<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
																<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
																	<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
																</xsl:apply-templates>
															</xsl:if>
															<xsl:variable name="altova:table">
																<table style="border:0px; border-top-style:none; width:100%; " border="1">
																	<xsl:variable name="altova:CurrContextGrid_22" select="."/>
																	<xsl:variable name="altova:ColumnData"/>
																	<tbody>
																		<xsl:for-each select="SpecifiedLogisticsServiceCharge">
																			<tr>
																				<td style="border-left-style:none; border-top-style:none; ">
																					<xsl:for-each select="Description">
																						<span style="white-space:pre-wrap; ">
																							<xsl:apply-templates/>
																						</span>
																					</xsl:for-each>
																				</td>
																				<td style="border-top-style:none; text-align:right; width:110px; ">
																					<xsl:for-each select="AppliedTradeTax">
																						<xsl:call-template name="TradeTax"/>
																					</xsl:for-each>
																				</td>
																				<td style="border-right-style:none; border-top-style:none; text-align:right; width:110px; ">
																					<xsl:for-each select="AppliedAmount">
																						<span>
																							<xsl:variable name="altova:seqContentStrings_23">
																								<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																							</xsl:variable>
																							<xsl:variable name="altova:sContent_23" select="string($altova:seqContentStrings_23)"/>
																							<xsl:value-of select="$altova:sContent_23"/>
																						</span>
																					</xsl:for-each>
																				</td>
																			</tr>
																		</xsl:for-each>
																	</tbody>
																</table>
															</xsl:variable>
															<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
															<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
															<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
																<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
															</xsl:apply-templates>
														</xsl:if>
													</xsl:if>
													<br/>
												</xsl:for-each>
											</xsl:for-each>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeSettlement"/>
											</xsl:for-each>
										</td>
									</tr>
									<tr>
										<td style="border-bottom-style:none; border-top-style:none; ">
											<xsl:if test="fn:exists(rsm:SpecifiedSupplyChainTradeTransaction/ApplicableSupplyChainTradeSettlement/ApplicableTradeTax)">
												<br/>
												<span style="font-weight:bold; text-decoration:underline; ">
													<xsl:text>Steuern:</xsl:text>
												</span>
												<br/>
												<br/>
												<xsl:variable name="altova:table">
													<table style="border:0px; border-collapse:collapse; border-right-style:none; width:100%; " border="1">
														<xsl:variable name="altova:CurrContextGrid_24" select="."/>
														<xsl:variable name="altova:ColumnData"/>
														<thead>
															<tr>
																<th style="border-left-style:none; ">
																	<span>
																		<xsl:text>Art (Kategorie)</xsl:text>
																	</span>
																</th>
																<th>
																	<span>
																		<xsl:text>Warenwert</xsl:text>
																	</span>
																</th>
																<th>
																	<span>
																		<xsl:text>Zu-/Abschlag</xsl:text>
																	</span>
																</th>
																<th style="width:110px; ">
																	<span>
																		<xsl:text>Nettobetrag</xsl:text>
																	</span>
																</th>
																<th style="width:110px; ">
																	<span>
																		<xsl:text>USt. %</xsl:text>
																	</span>
																</th>
																<th style="border-right-style:none; width:110px; ">
																	<span>
																		<xsl:text>USt.-Betrag</xsl:text>
																	</span>
																</th>
															</tr>
														</thead>
														<tbody>
															<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
																<xsl:for-each select="ApplicableSupplyChainTradeSettlement">
																	<xsl:for-each select="ApplicableTradeTax">
																		<tr>
																			<td style="border-left-style:none; ">
																				<xsl:for-each select="TypeCode">
																					<xsl:apply-templates/>
																				</xsl:for-each>
																				<span>
																					<xsl:text>&#160;</xsl:text>
																				</span>
																				<xsl:for-each select="CategoryCode">
																					<span>
																						<xsl:text>(</xsl:text>
																					</span>
																					<xsl:apply-templates/>
																					<span>
																						<xsl:text>)</xsl:text>
																					</span>
																				</xsl:for-each>
																				<xsl:for-each select="ExemptionReason">
																					<br/>
																					<xsl:apply-templates/>
																				</xsl:for-each>
																			</td>
																			<td style="text-align:right; "/>
																			<td style="text-align:right; "/>
																			<td style="text-align:right; width:110px; ">
																				<xsl:for-each select="BasisAmount">
																					<span>
																						<xsl:variable name="altova:seqContentStrings_25">
																							<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																						</xsl:variable>
																						<xsl:variable name="altova:sContent_25" select="string($altova:seqContentStrings_25)"/>
																						<xsl:value-of select="$altova:sContent_25"/>
																					</span>
																				</xsl:for-each>
																			</td>
																			<td style="text-align:right; width:110px; ">
																				<xsl:for-each select="ApplicablePercent">
																					<span>
																						<xsl:variable name="altova:seqContentStrings_26">
																							<xsl:value-of select="format-number(number(string(.)), '###.##0,####', 'format1')"/>
																						</xsl:variable>
																						<xsl:variable name="altova:sContent_26" select="string($altova:seqContentStrings_26)"/>
																						<xsl:value-of select="$altova:sContent_26"/>
																					</span>
																				</xsl:for-each>
																			</td>
																			<td style="border-right-style:none; text-align:right; width:110px; ">
																				<xsl:for-each select="CalculatedAmount">
																					<span>
																						<xsl:variable name="altova:seqContentStrings_27">
																							<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																						</xsl:variable>
																						<xsl:variable name="altova:sContent_27" select="string($altova:seqContentStrings_27)"/>
																						<xsl:value-of select="$altova:sContent_27"/>
																					</span>
																				</xsl:for-each>
																			</td>
																		</tr>
																	</xsl:for-each>
																</xsl:for-each>
															</xsl:for-each>
														</tbody>
													</table>
												</xsl:variable>
												<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
												<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
												<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
													<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
												</xsl:apply-templates>
											</xsl:if>
										</td>
									</tr>
									<tr>
										<td style="border-bottom-style:none; text-align:left; ">
											<br/>
											<span style="font-weight:bold; text-decoration:underline; ">
												<xsl:text>Belegsummen:</xsl:text>
											</span>
											<br/>
											<xsl:for-each select="rsm:SpecifiedSupplyChainTradeTransaction">
												<xsl:for-each select="ApplicableSupplyChainTradeSettlement">
													<xsl:for-each select="SpecifiedTradeSettlementMonetarySummation">
														<xsl:variable name="altova:table">
															<table style="border:0px; " border="1" width="100%">
																<xsl:variable name="altova:CurrContextGrid_28" select="."/>
																<xsl:variable name="altova:ColumnData"/>
																<tbody>
																	<tr>
																		<td style="border-style:none; "/>
																		<td style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; padding:1px; text-align:left; width:220px; ">
																			<span style="font-weight:bold; ">
																				<xsl:text>Positionssumme</xsl:text>
																			</span>
																		</td>
																		<td style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; padding:1px; text-align:right; width:110px; ">
																			<xsl:for-each select="LineTotalAmount">
																				<span style="font-weight:bold; ">
																					<xsl:variable name="altova:seqContentStrings_29">
																						<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																					</xsl:variable>
																					<xsl:variable name="altova:sContent_29" select="string($altova:seqContentStrings_29)"/>
																					<xsl:value-of select="$altova:sContent_29"/>
																				</span>
																			</xsl:for-each>
																		</td>
																	</tr>
																	<tr>
																		<td style="border-style:none; "/>
																		<td style="border:0px; border-top-style:double; padding:1px; text-align:left; width:220px; ">
																			<span>
																				<xsl:text>Gesamtbetrag der Zuschläge</xsl:text>
																			</span>
																		</td>
																		<td style="border:0px; border-top-style:double; padding:1px; text-align:right; width:110px; ">
																			<xsl:for-each select="ChargeTotalAmount">
																				<span>
																					<xsl:variable name="altova:seqContentStrings_30">
																						<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																					</xsl:variable>
																					<xsl:variable name="altova:sContent_30" select="string($altova:seqContentStrings_30)"/>
																					<xsl:value-of select="$altova:sContent_30"/>
																				</span>
																			</xsl:for-each>
																		</td>
																	</tr>
																	<tr>
																		<td style="border-style:none; "/>
																		<td style="border:0px; border-top-style:double; padding:1px; text-align:left; width:220px; ">
																			<span>
																				<xsl:text>Gesamtbetrag der Abschläge</xsl:text>
																			</span>
																		</td>
																		<td style="border:0px; border-top-style:double; padding:1px; text-align:right; width:110px; ">
																			<xsl:for-each select="AllowanceTotalAmount">
																				<span>
																					<xsl:variable name="altova:seqContentStrings_31">
																						<xsl:value-of select="format-number(number(-1*.), '##0,00', 'format1')"/>
																					</xsl:variable>
																					<xsl:variable name="altova:sContent_31" select="string($altova:seqContentStrings_31)"/>
																					<xsl:value-of select="$altova:sContent_31"/>
																				</span>
																			</xsl:for-each>
																		</td>
																	</tr>
																	<tr>
																		<td style="border-style:none; "/>
																		<td style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; padding:1px; text-align:left; width:220px; ">
																			<span style="font-weight:bold; ">
																				<xsl:text>Rechnungssumme ohne USt.</xsl:text>
																			</span>
																		</td>
																		<td style="border-bottom-style:solid; border-left-style:none; border-right-style:none; border-top-style:none; padding:1px; text-align:right; width:110px; ">
																			<xsl:for-each select="TaxBasisTotalAmount">
																				<span style="font-weight:bold; ">
																					<xsl:variable name="altova:seqContentStrings_32">
																						<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																					</xsl:variable>
																					<xsl:variable name="altova:sContent_32" select="string($altova:seqContentStrings_32)"/>
																					<xsl:value-of select="$altova:sContent_32"/>
																				</span>
																			</xsl:for-each>
																		</td>
																	</tr>
																	<tr>
																		<td style="border-style:none; "/>
																		<td style="border:0px; border-top-style:double; padding:1px; text-align:left; width:220px; ">
																			<span>
																				<xsl:text>Steuerbetrag</xsl:text>
																			</span>
																		</td>
																		<td style="border:0px; border-top-style:double; padding:1px; text-align:right; width:110px; ">
																			<xsl:for-each select="TaxTotalAmount">
																				<span>
																					<xsl:variable name="altova:seqContentStrings_33">
																						<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																					</xsl:variable>
																					<xsl:variable name="altova:sContent_33" select="string($altova:seqContentStrings_33)"/>
																					<xsl:value-of select="$altova:sContent_33"/>
																				</span>
																			</xsl:for-each>
																		</td>
																	</tr>
																	<tr>
																		<td style="border-style:none; "/>
																		<td style="border-bottom-style:double; border-bottom-width:medium; border-left-style:none; border-right-style:none; border-top-style:none; padding:1px; text-align:left; width:220px; ">
																			<span style="font-weight:bold; ">
																				<xsl:text>Bruttosumme</xsl:text>
																			</span>
																		</td>
																		<td style="border-bottom-style:double; border-bottom-width:medium; border-left-style:none; border-right-style:none; border-top-style:none; padding:1px; text-align:right; width:110px; ">
																			<xsl:for-each select="GrandTotalAmount">
																				<span style="font-weight:bold; ">
																					<xsl:variable name="altova:seqContentStrings_34">
																						<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																					</xsl:variable>
																					<xsl:variable name="altova:sContent_34" select="string($altova:seqContentStrings_34)"/>
																					<xsl:value-of select="$altova:sContent_34"/>
																				</span>
																			</xsl:for-each>
																		</td>
																	</tr>
																	<tr>
																		<td style="border-style:none; "/>
																		<td style="border:0px; border-top-style:double; padding:1px; text-align:left; width:220px; ">
																			<xsl:if test="(ram:TotalPrepaidAmount&gt;=0 and fn:exists(ram:TotalPrepaidAmount)) or(fn:not(fn:exists(ram:TotalPrepaidAmount)) and  fn:not(ends-with( ../../../rsm:SpecifiedExchangedDocumentContext/ram:GuidelineSpecifiedDocumentContextParameter/ram:ID ,  &apos;basic&apos; )))">
																				<span>
																					<xsl:text>Erhaltene Anzahlungen</xsl:text>
																				</span>
																				<br/>
																			</xsl:if>
																			<xsl:if test="ram:TotalPrepaidAmount&lt;0 and   fn:exists(ram:TotalPrepaidAmount)">
																				<span>
																					<xsl:text>Offene Zahlungen</xsl:text>
																				</span>
																				<br/>
																			</xsl:if>
																		</td>
																		<td style="border:0px; border-top-style:double; padding:1px; text-align:right; width:110px; ">
																			<xsl:if test="fn:exists(TotalPrepaidAmount) or fn:not(ends-with( ../../../rsm:SpecifiedExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID ,  &apos;basic&apos; ))">
																				<xsl:for-each select="TotalPrepaidAmount">
																					<span>
																						<xsl:variable name="altova:seqContentStrings_35">
																							<xsl:value-of select="format-number(number(-1*.), '##0,00', 'format1')"/>
																						</xsl:variable>
																						<xsl:variable name="altova:sContent_35" select="string($altova:seqContentStrings_35)"/>
																						<xsl:value-of select="$altova:sContent_35"/>
																					</span>
																				</xsl:for-each>
																			</xsl:if>
																		</td>
																	</tr>
																	<tr>
																		<td style="border-style:none; "/>
																		<td style="border:0px; border-top-style:double; padding:1px; text-align:left; width:220px; ">
																			<xsl:if test="fn:exists(DuePayableAmount) or fn:not(ends-with( ../../../rsm:SpecifiedExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID ,  &apos;basic&apos; ))">
																				<span style="font-weight:bold; ">
																					<xsl:text>Zahlbetrag</xsl:text>
																				</span>
																			</xsl:if>
																		</td>
																		<td style="border:0px; border-top-style:double; padding:1px; text-align:right; width:110px; ">
																			<xsl:if test="fn:exists(DuePayableAmount) or fn:not(ends-with( ../../../rsm:SpecifiedExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID ,  &apos;basic&apos; ))">
																				<xsl:for-each select="DuePayableAmount">
																					<span style="font-weight:bold; ">
																						<xsl:variable name="altova:seqContentStrings_36">
																							<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
																						</xsl:variable>
																						<xsl:variable name="altova:sContent_36" select="string($altova:seqContentStrings_36)"/>
																						<xsl:value-of select="$altova:sContent_36"/>
																					</span>
																				</xsl:for-each>
																			</xsl:if>
																		</td>
																	</tr>
																</tbody>
															</table>
														</xsl:variable>
														<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
														<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
														<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
															<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
														</xsl:apply-templates>
													</xsl:for-each>
												</xsl:for-each>
											</xsl:for-each>
										</td>
									</tr>
								</tbody>
							</table>
						</xsl:variable>
						<xsl:variable name="altova:col-count" select="sum( for $altova:cell in $altova:table/table/(thead | tbody | tfoot)[ 1 ]/tr[ 1 ]/(th | td) return altova:col-span( $altova:cell ) )"/>
						<xsl:variable name="altova:generate-cols" as="xs:boolean*" select="for $altova:pos in 1 to $altova:col-count return true()"/>
						<xsl:apply-templates select="$altova:table" mode="altova:generate-table">
							<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
						</xsl:apply-templates>
						<br/>
					</xsl:for-each>
				</xsl:for-each>
				<br/>
				<br/>
			</body>
		</html>
	</xsl:template>
	<xsl:template name="Party">
		<xsl:for-each select="ID">
			<span>
				<xsl:text>Nummer&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
			</span>
			<xsl:apply-templates/>
		</xsl:for-each>
		<xsl:for-each select="@schemeID">
			<xsl:call-template name="local_Identifier"/>
		</xsl:for-each>
		<xsl:for-each select="GlobalID">
			<br/>
			<span>
				<xsl:text>Globale Nummer: </xsl:text>
			</span>
			<xsl:apply-templates/>
			<xsl:for-each select="@schemeID">
				<span>
					<xsl:text>&#160;</xsl:text>
				</span>
				<xsl:call-template name="Global_Identifier"/>
			</xsl:for-each>
		</xsl:for-each>
		<xsl:for-each select="Name">
			<br/>
			<span style="font-weight:bold; white-space:pre-wrap; ">
				<xsl:apply-templates/>
			</span>
		</xsl:for-each>
		<xsl:for-each select="DefinedTradeContact">
			<xsl:for-each select="PersonName">
				<br/>
				<xsl:apply-templates/>
			</xsl:for-each>
			<xsl:for-each select="DepartmentName">
				<br/>
				<xsl:apply-templates/>
			</xsl:for-each>
		</xsl:for-each>
		<xsl:for-each select="PostalTradeAddress">
			<xsl:for-each select="LineOne">
				<br/>
				<xsl:apply-templates/>
			</xsl:for-each>
			<xsl:for-each select="LineTwo">
				<br/>
				<xsl:apply-templates/>
			</xsl:for-each>
			<xsl:for-each select="CountryID">
				<br/>
				<xsl:apply-templates/>
				<span>
					<xsl:text>&#160;</xsl:text>
				</span>
			</xsl:for-each>
			<xsl:for-each select="PostcodeCode">
				<xsl:apply-templates/>
				<span>
					<xsl:text>&#160;</xsl:text>
				</span>
			</xsl:for-each>
			<xsl:for-each select="CityName">
				<xsl:apply-templates/>
			</xsl:for-each>
		</xsl:for-each>
		<br/>
		<xsl:for-each select="DefinedTradeContact">
			<xsl:for-each select="TelephoneUniversalCommunication">
				<xsl:for-each select="CompleteNumber">
					<br/>
					<span>
						<xsl:text>Telefon&#160;&#160;&#160;&#160;&#160; : </xsl:text>
					</span>
					<xsl:apply-templates/>
				</xsl:for-each>
			</xsl:for-each>
			<xsl:for-each select="FaxUniversalCommunication">
				<xsl:for-each select="CompleteNumber">
					<br/>
					<span>
						<xsl:text>Fax&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
					</span>
					<xsl:apply-templates/>
				</xsl:for-each>
			</xsl:for-each>
			<xsl:for-each select="EmailURIUniversalCommunication">
				<xsl:for-each select="URIID">
					<br/>
					<span>
						<xsl:text>E-Mail&#160;&#160;&#160;&#160;&#160;&#160; : </xsl:text>
					</span>
					<xsl:apply-templates/>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:for-each>
		<xsl:for-each select="SpecifiedTaxRegistration">
			<xsl:for-each select="ID">
				<br/>
				<xsl:call-template name="TaxRegistration"/>
			</xsl:for-each>
		</xsl:for-each>
		<br/>
	</xsl:template>
	<xsl:template name="TradeTax">
		<xsl:for-each select="ApplicablePercent">
			<span>
				<xsl:variable name="altova:seqContentStrings_37">
					<xsl:value-of select="format-number(number(string(.)), '###.##0,####', 'format1')"/>
				</xsl:variable>
				<xsl:variable name="altova:sContent_37" select="string($altova:seqContentStrings_37)"/>
				<xsl:value-of select="$altova:sContent_37"/>
			</span>
			<span>
				<xsl:text> %</xsl:text>
			</span>
		</xsl:for-each>
		<xsl:for-each select="TypeCode">
			<span>
				<xsl:text>&#160;</xsl:text>
			</span>
			<xsl:apply-templates/>
		</xsl:for-each>
		<xsl:for-each select="CategoryCode">
			<span>
				<xsl:text> (</xsl:text>
			</span>
			<xsl:apply-templates/>
			<span>
				<xsl:text>)</xsl:text>
			</span>
		</xsl:for-each>
		<xsl:for-each select="CalculatedAmount">
			<span>
				<xsl:text> entspricht </xsl:text>
			</span>
			<span>
				<xsl:variable name="altova:seqContentStrings_38">
					<xsl:value-of select="format-number(number(string(.)), '##0,00', 'format1')"/>
				</xsl:variable>
				<xsl:variable name="altova:sContent_38" select="string($altova:seqContentStrings_38)"/>
				<xsl:value-of select="$altova:sContent_38"/>
			</span>
		</xsl:for-each>
		<xsl:for-each select="ExemptionReason">
			<br/>
			<xsl:apply-templates/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="Positionsreferenz">
		<xsl:for-each select="ID">
			<span>
				<xsl:text> Nr. </xsl:text>
			</span>
			<xsl:apply-templates/>
		</xsl:for-each>
		<xsl:for-each select="IssueDateTime">
			<span>
				<xsl:text> vom </xsl:text>
			</span>
			<xsl:call-template name="Datum"/>
		</xsl:for-each>
		<xsl:for-each select="LineID">
			<span>
				<xsl:text> Zeile </xsl:text>
			</span>
			<xsl:apply-templates/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="Datum">
		<span>
			<xsl:value-of select="if ( @format = &apos;102&apos;) then concat(substring(.,7,2),&apos;.&apos;,substring(.,5,2),&apos;.&apos;,substring(.,1,4)) else 
if ( @format = &apos;610&apos;) then concat(substring(.,5,2),&apos;-&apos;,substring(.,1,4)) else
if ( @format = &apos;616&apos;) then concat(&apos;KW &apos;, substring(.,5,2),&apos; &apos;,substring(.,1,4)) else
concat(&apos;Ungültiges Datumsformat für: &apos;, .)"/>
		</span>
	</xsl:template>
	<xsl:template name="Versand_Identifer">
		<span>
			<xsl:value-of select="if(. eq &apos;GSIN&apos;) then . else
if(. eq &apos;GINC&apos;) then . else
if(. eq &apos;SSCC&apos;) then &apos;Nummer der Versandeinheit&apos; else
if(. eq &apos;FLIGHT_NO&apos;) then &apos;Flugnummer&apos; else
if(. eq &apos;NUMBER_PLATE&apos;) then &apos;Nummernschild&apos; else
if(. eq &apos;SHIPMENT_REFERENCE&apos;) then &apos;Sendungs-/Ladungsbezugsnummer&apos; else
."/>
		</span>
	</xsl:template>
	<xsl:template name="Global_Identifier">
		<span>
			<xsl:text>(</xsl:text>
		</span>
		<span>
			<xsl:value-of select="if (. eq &apos;0021&apos;) then &apos;BIC&apos; else
if (. eq &apos;0060&apos;) then &apos;DUNS&apos; else
if (. eq &apos;0088&apos;) then &apos;GTIN&apos; else 
if (. eq &apos;0160&apos;) then &apos;GLN&apos; else
if (. eq &apos;0177&apos;) then &apos;OSCAR&apos; else
."/>
		</span>
		<span>
			<xsl:text>)</xsl:text>
		</span>
	</xsl:template>
	<xsl:template name="Menge">
		<span>
			<xsl:variable name="altova:seqContentStrings_39">
				<xsl:value-of select="format-number(number(string(.)), '##0', 'format1')"/>
			</xsl:variable>
			<xsl:variable name="altova:sContent_39" select="string($altova:seqContentStrings_39)"/>
			<xsl:value-of select="$altova:sContent_39"/>
		</span>
		<xsl:for-each select="@unitCode">
			<span>
				<xsl:text>&#160;</xsl:text>
			</span>
			<span>
				<xsl:value-of select="if (. eq &apos;C62&apos;) then &apos;Stk&apos; else 
if (. eq &apos;DAY&apos;) then &apos;Tag(e)&apos; else
if (. eq &apos;HAR&apos;) then &apos;ha&apos; else
if (. eq &apos;HUR&apos;) then &apos;Std&apos; else
if (. eq &apos;KGM&apos;) then &apos;kg&apos; else
if (. eq &apos;KTM&apos;) then &apos;km&apos; else
if (. eq &apos;LS&apos;) then &apos;pausch&apos; else
if (. eq &apos;LTR&apos;) then &apos;l&apos; else
if (. eq &apos;MIN&apos;) then &apos;min&apos; else
if (. eq &apos;MMK&apos;) then &apos;mm²&apos; else
if (. eq &apos;MMT&apos;) then &apos;mm&apos; else
if (. eq &apos;MTK&apos;) then &apos;m²&apos; else
if (. eq &apos;MTQ&apos;) then &apos;m³&apos; else
if (. eq &apos;MTR&apos;) then &apos;m&apos; else
if (. eq &apos;NAR&apos;) then &apos;Anz&apos; else
if (. eq &apos;NPR&apos;) then &apos;Paar&apos; else
if (. eq &apos;P1&apos;) then &apos;%&apos; else
if (. eq &apos;SET&apos;) then &apos;Set&apos; else
if (. eq &apos;TNE&apos;) then &apos;t&apos; else
if (. eq &apos;WEE&apos;) then &apos;Woche(n)&apos; else
."/>
			</span>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="TaxRegistration">
		<span>
			<xsl:value-of select="if ( @schemeID = &apos;FC&apos;) then concat(&apos;Steuernummer  : &apos;, .) else 
if ( @schemeID = &apos;VA&apos;) then concat(&apos;USt.-Id.-Nr.  : &apos;, .) else
concat(&apos;Unbekannte Steuernummer (&apos;, @schemeID , &apos;): &apos;, .)"/>
		</span>
	</xsl:template>
	<xsl:template name="VersandMode">
		<span>
			<xsl:value-of select="if (. eq &apos;0&apos;)then &apos;0 - Nicht spezifiziert&apos; else
if (. eq &apos;1&apos;)then &apos;1 - Seefracht&apos; else
if (. eq &apos;2&apos;)then &apos;2 - Schiene&apos; else
if (. eq &apos;3&apos;)then &apos;3 - Straße&apos; else
if (. eq &apos;4&apos;)then &apos;4 - Luftfracht&apos; else
if (. eq &apos;5&apos;)then &apos;5 - Post/Paketversand&apos; else
if (. eq &apos;6&apos;)then &apos;6 - Multimodaler Transport/kombinierter Verkehr&apos; else
if (. eq &apos;7&apos;)then &apos;7 - Feste Transportinstallation&apos; else
if (. eq &apos;8&apos;)then &apos;8 - Transport auf Binnengewässern&apos; else
if (. eq &apos;9&apos;)then &apos;9 - Nicht anwendbar&apos; else
fn:concat(.,&apos; - unbekannte Transportart&apos;)"/>
		</span>
	</xsl:template>
	<xsl:template name="PaymentType">
		<span>
			<xsl:value-of select="if(. eq &apos;1&apos;) then concat(.,&apos; - nicht definiert&apos;) else
if(. eq &apos;3&apos;) then concat(.,&apos; - Belastung durch automatisierte Clearingstelle&apos;) else
if(. eq &apos;10&apos;) then concat(.,&apos; - Barzahlung&apos;) else
if(. eq &apos;20&apos;) then concat(.,&apos; - per Scheck&apos;) else
if(. eq &apos;31&apos;) then concat(.,&apos; - per Überweisung (SEPA)&apos;) else
if(. eq &apos;42&apos;) then concat(.,&apos; - per Überweisung (nicht SEPA)&apos;) else
if(. eq &apos;48&apos;) then concat(.,&apos; - per Kreditkarte&apos;) else
if(. eq &apos;49&apos;) then concat(.,&apos; - per Lastschrift&apos;) else
if(. eq &apos;97&apos;) then concat(.,&apos; - per Ausgleich zwischen Partnern&apos;) else
."/>
		</span>
	</xsl:template>
	<xsl:template name="Referenz">
		<span>
			<xsl:value-of select="if (. eq &apos;AAA&apos;)then &apos;Auftragsbestätigung&apos; else
if (. eq &apos;AAB&apos;)then &apos;Proforma-Rechnung&apos; else
if (. eq &apos;AAG&apos;) then &apos;Angebot&apos; else
if (. eq &apos;AAJ&apos;) then &apos;Lieferauftrag&apos; else
if (. eq &apos;AAL&apos;) then &apos;Zeichnung&apos; else
if (. eq &apos;AAM&apos;) then &apos;Frachtbrief&apos; else
if (. eq &apos;AAS&apos;) then &apos;Transportdokument&apos; else
if (. eq &apos;ABT&apos;) then &apos;Zollerklärung&apos; else
if (. eq &apos;AER&apos;) then &apos;Projektspezifikation&apos; else
if (. eq &apos;AGG&apos;) then &apos;Reklamation&apos; else
if (. eq &apos;AJS&apos;) then &apos;Vereinbarung&apos; else
if (. eq &apos;ALO&apos;) then &apos;Wareneingang&apos; else
if (. eq &apos;ALQ&apos;) then &apos;Rücksendungsanzeige&apos; else
if (. eq &apos;API&apos;) then &apos;Bestandsbericht&apos; else
if (. eq &apos;ASI&apos;) then &apos;Abliefernachweis&apos; else
if (. eq &apos;AUD&apos;) then &apos;Inkasso-Referenz&apos; else
if (. eq &apos;AWR&apos;) then &apos;Ursprungsbeleg&apos; else
if (. eq &apos;BO&apos;) then &apos;Rahmenauftrag&apos; else
if (. eq &apos;BC&apos;) then &apos;Vertrag (Käufer)&apos; else
if (. eq &apos;CD&apos;) then &apos;Gutschrift&apos; else
if (. eq &apos;DL&apos;) then &apos;Belastungsanzeige&apos; else
if (. eq &apos;MG&apos;) then &apos;Zählernummer&apos; else
if (. eq &apos;OI&apos;) then &apos;Vorherige Rechnung&apos; else
if (. eq &apos;PK&apos;) then &apos;Packliste&apos; else
if (. eq &apos;PL&apos;) then &apos;Preisliste&apos; else
if (. eq &apos;POR&apos;) then &apos;Bestellantwort&apos; else
if (. eq &apos;PP&apos;) then &apos;Bestelländerung&apos; else
if (. eq &apos;TIN&apos;) then &apos;Transportauftrag&apos; else
if (. eq &apos;VN&apos;) then &apos;Auftragsnummer (Lieferant)&apos; else
."/>
		</span>
	</xsl:template>
	<xsl:template name="PackageQuantity">
		<xsl:for-each select="PackageQuantity">
			<xsl:for-each select="@unitCode">
				<span>
					<xsl:text>&#160;</xsl:text>
				</span>
				<span>
					<xsl:value-of select="if (. eq &apos;BA&apos;) then &apos;Tonne&apos; else 
if (. eq &apos;BC&apos;) then &apos;Getränkekiste&apos;  else 
if (. eq &apos;BG&apos;) then &apos;Tüte, Beutel&apos;  else 
if (. eq &apos;BO&apos;) then &apos;Flasche&apos;  else 
if (. eq &apos;BX&apos;) then &apos;Schachtel&apos;  else 
if (. eq &apos;CS&apos;) then &apos;Kiste&apos;  else 
if (. eq &apos;CT&apos;) then &apos;Karton&apos;  else 
if (. eq &apos;CX&apos;) then &apos;Dose&apos;  else 
if (. eq &apos;NE&apos;) then &apos;Unverpackt oder ausgepackt&apos;  else 
if (. eq &apos;PX&apos;) then &apos;Palette&apos;  else 
if (. eq &apos;RO&apos;) then &apos;Rolle&apos;  else 
if (. eq &apos;SA&apos;) then &apos;Sack&apos;  
else ."/>
				</span>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="local_Identifier">
		<span>
			<xsl:text>(</xsl:text>
		</span>
		<xsl:choose>
			<xsl:when test=". instance of element()">
				<xsl:apply-templates/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
		<span>
			<xsl:text>)</xsl:text>
		</span>
	</xsl:template>
	<xsl:template name="Klassifikationsart">
		<xsl:for-each select="@listID">
			<span>
				<xsl:text>, Art: </xsl:text>
			</span>
			<span>
				<xsl:value-of select="if (. eq &apos;GPS&apos;) then &apos;Global Product Classification (GS1)&apos; else
if (. eq &apos;ECL&apos;) then &apos;eCl@ass&apos; else
if (. eq &apos;UNSPSC&apos;) then &apos;United Nations Standard Products and Services Code® (UNSPSC®)&apos; else 
if (. eq &apos;HS&apos;) then &apos;Zolltarifnummer (Harmonised System)&apos; else
if (. eq &apos;CBV&apos;) then &apos;Gemeinsames Vokabular für öffentliche Aufträge (Common Procurement Vocabulary - CPV)&apos; else
if (. eq &apos;SELLER_ASSIGNED&apos;) then &apos;vom Verkäufer vergeben&apos; else
if (. eq &apos;BUYER_ASSIGNED&apos;) then &apos;vom Käufer vergeben&apos; else
."/>
			</span>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="DeliveryTypeCode">
		<span>
			<xsl:value-of select="if (. eq &apos;XW&apos;) then &apos;ab Werk&apos; else
if (. eq &apos;FCA&apos;) then &apos;frei Frachtführer&apos; else
if (. eq &apos;FAS&apos;) then &apos;frei längsseits Schiff&apos; else 
if (. eq &apos;FOB&apos;) then &apos;frei an Bord&apos; else
if (. eq &apos;CFR&apos;) then &apos;Kosten und Fracht&apos; else
if (. eq &apos;CIF&apos;) then &apos;Kosten, Versicherung und Fracht bis zum Bestimmungshafen&apos; else
if (. eq &apos;DAT&apos;) then &apos;geliefert Terminal&apos; else
if (. eq &apos;DAP&apos;) then &apos;geliefert benannter Ort&apos; else
if (. eq &apos;CPT&apos;) then &apos;Fracht bezahlt bis&apos; else
if (. eq &apos;CIP&apos;) then &apos;Fracht und Versicherung bezahlt&apos; else
if (. eq &apos;DDP&apos;) then &apos;gelifert Zoll bezahlt&apos; else
."/>
		</span>
	</xsl:template>
	<xsl:template name="AllowanceChargeReasonCode">
		<span>
			<xsl:text>(</xsl:text>
		</span>
		<span>
			<xsl:value-of select="if (. eq &apos;AA&apos;) then &apos;Werbekostenzuschuß&apos; else
if (. eq &apos;ADR&apos;) then &apos;Andere Dienste&apos; else
if (. eq &apos;AEO&apos;) then &apos;Sammel- und Recyclingservice&apos; else 
if (. eq &apos;DI&apos;) then &apos;Abzug (Rabatt)&apos; else
if (. eq &apos;EAB&apos;) then &apos;Skonto&apos; else
if (. eq &apos;FC&apos;) then &apos;Frachtgebühren&apos; else
if (. eq &apos;IN&apos;) then &apos;Versicherung&apos; else
if (. eq &apos;MAC&apos;) then &apos;Mindermengenzuschlag&apos; else
if (. eq &apos;NAA&apos;) then &apos;Einwegbehälter&apos; else
if (. eq &apos;PC&apos;) then &apos;Verpacken&apos; else
if (. eq &apos;RAA&apos;) then &apos;Rückvergütung&apos; else
if (. eq &apos;SH&apos;) then &apos;Spezielle Handhabungsdienstleistungen&apos; else
."/>
		</span>
		<span>
			<xsl:text>)</xsl:text>
		</span>
	</xsl:template>
	<xsl:function name="altova:is-cell-empty" as="xs:boolean">
		<xsl:param name="altova:cell" as="element()"/>
		<xsl:sequence select="altova:is-node-empty( $altova:cell )"/>
	</xsl:function>
	<xsl:function name="altova:is-node-empty" as="xs:boolean">
		<xsl:param name="altova:node" as="element()"/>
		<xsl:sequence select="every $altova:child in $altova:node/child::node() satisfies ( ( boolean( $altova:child/self::text() ) and string-length( $altova:child ) = 0 ) or ( ( boolean( $altova:child/self::div ) or boolean( $altova:child/self::span ) or boolean( $altova:child/self::a ) ) and altova:is-node-empty( $altova:child ) ) )"/>
	</xsl:function>
	<xsl:function name="altova:col-span" as="xs:integer">
		<xsl:param name="altova:cell" as="element()"/>
		<xsl:sequence select="if ( exists( $altova:cell/@colspan ) ) then xs:integer( $altova:cell/@colspan ) else 1"/>
	</xsl:function>
	<xsl:template match="@* | node()" mode="altova:generate-table">
		<xsl:param name="altova:generate-cols"/>
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="#current">
				<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="tbody" mode="altova:generate-table">
		<xsl:param name="altova:generate-cols"/>
		<xsl:choose>
			<xsl:when test="empty(tr)">
				<xsl:copy>
					<tr>
						<td/>
					</tr>
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" mode="#current">
						<xsl:with-param name="altova:generate-cols" select="$altova:generate-cols"/>
					</xsl:apply-templates>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="th | td" mode="altova:generate-table">
		<xsl:choose>
			<xsl:when test="altova:is-cell-empty( . )">
				<xsl:copy>
					<xsl:apply-templates select="@*" mode="#current"/>
					<xsl:text>&#160;</xsl:text>
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy>
					<xsl:apply-templates select="@* | node()" mode="#current"/>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:function name="altova:GetChartYValuesForSingleSeries">
		<xsl:param name="seqCategoryLeafPos" as="node()*"/>
		<xsl:param name="nodeSeriesLeafPos" as="node()"/>
		<xsl:param name="bValuesInCategory" as="xs:boolean"/>
		<xsl:for-each select="$seqCategoryLeafPos">
			<xsl:element name="altova:Value">
				<xsl:value-of select="altova:GetChartYValueForSingleSeriesPos($nodeSeriesLeafPos, ., $bValuesInCategory)"/>
			</xsl:element>
		</xsl:for-each>
	</xsl:function>
	<xsl:function name="altova:GetChartYValueForSingleSeriesPos">
		<xsl:param name="nodeSeriesLeafPos" as="node()"/>
		<xsl:param name="nodeCategoryLeafPos" as="node()"/>
		<xsl:param name="bValuesInCategory" as="xs:boolean"/>
		<xsl:variable name="altova:seqCategoryContextIds" select="$nodeCategoryLeafPos/altova:Context/@altova:ContextId" as="xs:string*"/>
		<xsl:variable name="altova:seqSeriesContextIds" select="$nodeSeriesLeafPos/altova:Context/@altova:ContextId" as="xs:string*"/>
		<xsl:variable name="altova:sCommonContextId" select="for $i in $altova:seqCategoryContextIds return if (some $j in $altova:seqSeriesContextIds satisfies $i eq $j) then $i else ()" as="xs:string*"/>
		<xsl:choose>
			<xsl:when test="count($altova:sCommonContextId) gt 1">
				<xsl:message select="concat('Es wurden mehrere Werte anstatt eines einzigen gefunden (Contexts: ', string-join($altova:sCommonContextId, ', '), ').')" terminate="yes"/>
			</xsl:when>
			<xsl:when test="count($altova:sCommonContextId) lt 1">
				<xsl:message select="concat('XBRL Chart: Info: No value found for position labeled &quot;', $nodeCategoryLeafPos/@altova:sLabel, '&quot;')" terminate="no"/>
				<xsl:sequence select="'altova:no-value'"/>
			</xsl:when>
			<xsl:when test="$bValuesInCategory">
				<xsl:sequence select="xs:string($nodeCategoryLeafPos/altova:Context[@altova:ContextId eq $altova:sCommonContextId]/@altova:Value)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:sequence select="xs:string($nodeSeriesLeafPos/altova:Context[@altova:ContextId eq $altova:sCommonContextId]/@altova:Value)"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:function>
	<xsl:function name="altova:GetChartLabelForPos" as="xs:string">
		<xsl:param name="nodeParam" as="node()"/>
		<xsl:value-of select="string-join($nodeParam/ancestor-or-self::altova:Pos/@altova:sLabel, ' ')"/>
	</xsl:function>
	<xsl:function name="altova:convert-length-to-pixel" as="xs:decimal">
		<xsl:param name="altova:length"/>
		<xsl:variable name="normLength" select="normalize-space($altova:length)"/>
		<xsl:choose>
			<xsl:when test="ends-with($normLength, 'px')">
				<xsl:value-of select="substring-before($normLength, 'px')"/>
			</xsl:when>
			<xsl:when test="ends-with($normLength, 'in')">
				<xsl:value-of select="xs:decimal(substring-before($normLength, 'in')) * $altova:nPxPerIn"/>
			</xsl:when>
			<xsl:when test="ends-with($normLength, 'cm')">
				<xsl:value-of select="xs:decimal(substring-before($normLength, 'cm')) * $altova:nPxPerIn div 2.54"/>
			</xsl:when>
			<xsl:when test="ends-with($normLength, 'mm')">
				<xsl:value-of select="xs:decimal(substring-before($normLength, 'mm')) * $altova:nPxPerIn div 25.4"/>
			</xsl:when>
			<xsl:when test="ends-with($normLength, 'pt')">
				<xsl:value-of select="xs:decimal(substring-before($normLength, 'pt')) * $altova:nPxPerIn div 72.0"/>
			</xsl:when>
			<xsl:when test="ends-with($normLength, 'pc')">
				<xsl:value-of select="xs:decimal(substring-before($normLength, 'pc')) * $altova:nPxPerIn div 6.0"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$normLength"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:function>
	<xsl:function name="altova:convert-length-to-mm" as="xs:decimal">
		<xsl:param name="altova:length"/>
		<xsl:variable name="normLength" select="normalize-space($altova:length)"/>
		<xsl:choose>
			<xsl:when test="ends-with($normLength, 'px')">
				<xsl:value-of select="xs:decimal(substring-before($normLength, 'px')) div $altova:nPxPerIn * 25.4"/>
			</xsl:when>
			<xsl:when test="ends-with($normLength, 'in')">
				<xsl:value-of select="xs:decimal(substring-before($normLength, 'in')) * 25.4"/>
			</xsl:when>
			<xsl:when test="ends-with($normLength, 'cm')">
				<xsl:value-of select="xs:decimal(substring-before($normLength, 'cm')) * 10"/>
			</xsl:when>
			<xsl:when test="ends-with($normLength, 'mm')">
				<xsl:value-of select="substring-before($normLength, 'mm') "/>
			</xsl:when>
			<xsl:when test="ends-with($normLength, 'pt')">
				<xsl:value-of select="xs:decimal(substring-before($normLength, 'pt')) * 25.4 div 72.0"/>
			</xsl:when>
			<xsl:when test="ends-with($normLength, 'pc')">
				<xsl:value-of select="xs:decimal(substring-before($normLength, 'pc')) * 25.4 div 6.0"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="number($normLength) div $altova:nPxPerIn * 25.4"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:function>
</xsl:stylesheet>
