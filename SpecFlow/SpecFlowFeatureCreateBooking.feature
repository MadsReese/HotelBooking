Feature: SpecFlowFeatureCreateBooking
	Too earn money we want our guests to be able to book rooms

@mytag
Scenario Outline: Create Booking
	Given I have entered <StartDate> 
	And i have  also entered <EndDate> booking information on the create booking page
	| Id | StartDate  | EndDate    | IsActive | CustomerId | RoomId |
	| 0  | 2017-11-17 | 2017-11-23 | false    | 1          | 1      |
	| 1  | 2017-11-17 | 2017-11-22 | false    | 2          | 2      |
	
	
	And There are available rooms
	When I press Create
	Then Anew booking should be created
