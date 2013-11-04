Feature: SearchAppsGallery1
	

@mytag
Scenario: SearchAppsGallery
	Given the alteryx service is running at "http://gallery.alteryx.com"
	When I invoke GET at application details at "api/apps/gallery" for "Site Selection Demo"
    Then I see run count > 1
	And I see description contains "Choosing a location impacts the bottom line"

