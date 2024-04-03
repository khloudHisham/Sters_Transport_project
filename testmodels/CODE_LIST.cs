namespace StersTransport.testmodels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CODE_LIST
    {
        [Key]
        [StringLength(255)]
        public string Client_Code { get; set; }

        [StringLength(50)]
        public string BranchCode { get; set; }

        [StringLength(50)]
        public string YearCode { get; set; }

        public double? Shipment_No { get; set; }

        [StringLength(255)]
        public string SenderName { get; set; }

        [StringLength(255)]
        public string SenderCompany { get; set; }

        [StringLength(255)]
        public string Sender_ID { get; set; }

        [StringLength(255)]
        public string Sender_Tel { get; set; }

        [StringLength(255)]
        public string ReceiverName { get; set; }

        [StringLength(255)]
        public string ReceiverCompany { get; set; }

        [StringLength(255)]
        public string Receiver_Tel { get; set; }

        [StringLength(255)]
        public string Goods_Description { get; set; }

        public double? Goods_Value { get; set; }

        [StringLength(255)]
        public string Insurance_Yes_No { get; set; }

        public bool? Have_Insurance { get; set; }

        public double? Insurance_Percentage { get; set; }

        public double? Insurance_Amount { get; set; }

        public double? Pallet_No { get; set; }

        public double? Box_No { get; set; }

        public double? Weight_Kg { get; set; }

        [StringLength(255)]
        public string Weight_L_W_H_cm { get; set; }

        public double? Weight_Vol_Factor { get; set; }

        public double? Weight_Vol { get; set; }

        public double? Weight_Total { get; set; }

        public double? Admin_ExportDoc_Cost { get; set; }

        [StringLength(255)]
        public string DatePost { get; set; }

        [StringLength(255)]
        public string TimePost { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PostDate { get; set; }

        public double? Packiging_cost_IQD { get; set; }

        public double? Custom_Cost_IQD { get; set; }

        public double? POST_DoorToDoor_IQD { get; set; }

        public double? Sub_Post_Cost_IQD { get; set; }

        public double? Discount_Post_Cost_Send { get; set; }

        public double? Total_Post_Cost_IQD { get; set; }

        public double? TotalPaid_IQD { get; set; }

        public double? EuropaToPay { get; set; }

        public double? Currency_Rate_1_IQD { get; set; }

        [StringLength(255)]
        public string Currency_Type { get; set; }

        [StringLength(255)]
        public string PriceDoorToDoorEach10KG_IQD { get; set; }

        public decimal? PriceDoorToDoorEach10KG { get; set; }

        public double? Price_KG_IQD { get; set; }

        public double? StartPrice_1_to_7KG { get; set; }

        [StringLength(255)]
        public string CountryAgent { get; set; }

        [StringLength(255)]
        public string Agent { get; set; }

        [StringLength(255)]
        public string Street_Name_No { get; set; }

        [StringLength(255)]
        public string Dep_Appar { get; set; }

        [StringLength(255)]
        public string ZipCode { get; set; }

        [StringLength(255)]
        public string CityPost { get; set; }

        [StringLength(255)]
        public string CountryPost { get; set; }

        [StringLength(255)]
        public string Note_Send { get; set; }

        [StringLength(255)]
        public string Person_in_charge_Send { get; set; }

        [StringLength(255)]
        public string Agent_Name_KU { get; set; }

        [StringLength(255)]
        public string Local_Post_YES_NO { get; set; }

        public bool? Have_Local_Post { get; set; }

        public string Update_Send_KU { get; set; }

        [StringLength(255)]
        public string Receive_State { get; set; }

        [StringLength(255)]
        public string Receive_Date_Time { get; set; }

        [StringLength(255)]
        public string Note_Received { get; set; }

        [StringLength(255)]
        public string Agent_EU_ReceiverName { get; set; }

        [StringLength(255)]
        public string Person_in_charge_Receive { get; set; }

        [StringLength(255)]
        public string Received_Amount_EU { get; set; }

        [StringLength(255)]
        public string Discount_Post_Cost_Received { get; set; }

        [StringLength(255)]
        public string PaymentWAY_Cash_PIN_Bank { get; set; }

        [StringLength(255)]
        public string Update_Receive_EU { get; set; }

        [StringLength(50)]
        public string Sender_ID_Type { get; set; }

        public long? Num { get; set; }

        public decimal? CommissionKG { get; set; }

        public decimal? CommissionBox { get; set; }

        public long? Weight_L_cm { get; set; }

        public long? Weight_W_cm { get; set; }

        public long? Weight_H_cm { get; set; }

        public long? BranchId { get; set; }

        public long? UserId { get; set; }

        public long? CountryAgentId { get; set; }

        public long? CountryPostId { get; set; }

        public long? AgentId { get; set; }

        public long? CityPostId { get; set; }

        public long? Person_in_charge_Id { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] stamp { get; set; }

        public double? Custome_Cost_Qomrk { get; set; }

        public bool? IsImported { get; set; }

        public double? BoxPackigingFactor { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? PostYear { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoNumber { get; set; }

        public virtual IdentityType IdentityType { get; set; }
    }
}
