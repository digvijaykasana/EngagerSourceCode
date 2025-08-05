using EngagerMark4.ApplicationCore.Account.Cris;
using EngagerMark4.ApplicationCore.Account.IRepository;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Inventory.Cris;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.Inventory.IRepository.Price;
using EngagerMark4.ApplicationCore.Inventory.IService.Price;
using EngagerMark4.Common.Configs;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using EngagerMark4.Infrasturcture.Repository.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngagerMark4.Service.Inventory.Price
{
    public class PriceService : AbstractService<IPriceRepository, PriceCri, EngagerMark4.ApplicationCore.Inventory.Entities.Price>, IPriceService
    {
        ApplicationDbContext _context;
        IGeneralLedgerRepository _generalLedgerRepository;
        IPriceLocationRepository _priceLocationRepository;

        EngagerMark4.ApplicationCore.Inventory.Entities.Price viceVersaPriceEntity;


        public PriceService(IPriceRepository repository, ApplicationDbContext context, IGeneralLedgerRepository generalLedgerRepository, IPriceLocationRepository priceLoctionRepository) : base(repository)
        {
            this._context = context;
            this._generalLedgerRepository = generalLedgerRepository;
            this._priceLocationRepository = priceLoctionRepository;
        }

        public async Task<EngagerMark4.ApplicationCore.Inventory.Entities.Price> FindByPickUpAndDropOff(long pickUpLocationId, long dropOffLocationId, long customerId)
        {
            return await this.repository.FindByPickUpAndDropOff(pickUpLocationId, dropOffLocationId, customerId);
        }

        public async Task<EngagerMark4.ApplicationCore.Inventory.Entities.Price> GetByGLCodeId(long glCodeId, long customerId)
        {
            return await this.repository.GetByGLCodeId(glCodeId, customerId);
        }

        public async Task<Int32> ImportReturnTripChargesByCustomerId(Int64 customerId)
        {
            try
            {

                GeneralLedgerCri generalLedgerCri = new GeneralLedgerCri()
                {
                    SearchValue = GLCodesConfig.tripCharges
                };

                var generalLedgerCodes = await _generalLedgerRepository.GetByCri(generalLedgerCri);

                if (generalLedgerCodes != null && generalLedgerCodes.Count() > 0)
                {
                    var tripChargesCode = generalLedgerCodes.First();

                    var priceLists = this.repository.GetTripChargesByGLCodeAndCustomerId(tripChargesCode.Id, customerId);

                    if (priceLists != null && priceLists.Count() > 0)
                    {
                        this.repository.BeginTransaction();

                        int count = 0;

                        //var entity = priceLists.FirstOrDefault();
                        foreach (var entity in priceLists)
                        {
                            bool needToGenerateSerialForViceVersa = false;

                            viceVersaPriceEntity = null;

                            if (entity.ViceVersa && entity.PickUpPointId != 0 && entity.DropPointId != 0)
                            {
                                viceVersaPriceEntity = priceLists.Where(x => x.PickUpPointId == entity.DropPointId && x.DropPointId == entity.PickUpPointId && x.CustomerId == entity.CustomerId.Value).FirstOrDefault();

                                if (viceVersaPriceEntity == null)
                                {
                                    needToGenerateSerialForViceVersa = true;

                                    viceVersaPriceEntity = new EngagerMark4.ApplicationCore.Inventory.Entities.Price()
                                    {
                                        Name = entity.Name,
                                        CustomerId = entity.CustomerId,
                                        GLCodeId = entity.GLCodeId,
                                        IsTaxable = entity.IsTaxable,
                                        AssignedPrice = entity.AssignedPrice,
                                        PickUpPoint = entity.DropPoint,
                                        PickUpPointId = entity.DropPointId,
                                        DropPoint = entity.PickUpPoint,
                                        DropPointId = entity.PickUpPointId,
                                        DiscountPercent = entity.DiscountPercent,
                                        DiscountAmt = entity.DiscountAmt,
                                        MaxPax = entity.MaxPax,
                                        ExceedAmt = entity.ExceedAmt,
                                        StartTime = entity.StartTime,
                                        StartTimeBinding = entity.StartTimeBinding,
                                        EndTime = entity.EndTime,
                                        EndTimeBinding = entity.EndTimeBinding,
                                        ViceVersa = entity.ViceVersa
                                    };

                                    this.repository.Save(viceVersaPriceEntity);

                                    await this.repository.SaveChangesAsync();

                                    if (needToGenerateSerialForViceVersa && viceVersaPriceEntity != null)
                                    {
                                        SerialNoRepository<PriceSerialNo> serialNoRepoitory = new SerialNoRepository<PriceSerialNo>(_context);

                                        SystemSettingRepository settingsRepository = new SystemSettingRepository(_context);

                                        #region SystemSettingCri
                                        SystemSettingCri sysCri = new SystemSettingCri()
                                        {
                                            StringCris = new Dictionary<string, StringValue>()
                                        };
                                        #endregion

                                        sysCri.StringCris["Code"] = new StringValue { ComparisonOperator = BaseCri.StringComparisonOperator.Equal, Value = AppSettingKey.Key.PRICE_CODE_NUM_COUNT.ToString() };

                                        var setting = await settingsRepository.GetByCri(sysCri);

                                        viceVersaPriceEntity.Code = viceVersaPriceEntity.GetPriceCode(serialNoRepoitory.GetSerialNo(viceVersaPriceEntity.Id), Convert.ToInt64(setting.FirstOrDefault().Value));

                                    }

                                    await this.repository.SaveChangesAsync();

                                    count++;

                                    if (viceVersaPriceEntity.PickUpPointId != 0)
                                    {
                                        PriceLocation pickUpPoint = new PriceLocation()
                                        {
                                            PriceId = viceVersaPriceEntity.Id,
                                            LocationId = viceVersaPriceEntity.PickUpPointId,
                                            Description = String.Empty,
                                            Type = PriceLocation.PriceLocationType.PickUp,
                                        };

                                        this._priceLocationRepository.Save(pickUpPoint);
                                    }

                                    if (viceVersaPriceEntity.DropPointId != 0)
                                    {
                                        PriceLocation dropPoint = new PriceLocation()
                                        {
                                            PriceId = viceVersaPriceEntity.Id,
                                            LocationId = viceVersaPriceEntity.DropPointId,
                                            Description = String.Empty,
                                            Type = PriceLocation.PriceLocationType.DropOff,
                                        };

                                        this._priceLocationRepository.Save(dropPoint);
                                    }

                                    await this._priceLocationRepository.SaveChangesAsync();
                                }
                            }
                        }


                        this.repository.CommitTransaction();

                        return count;
                    }
                }

                return 0;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async override Task<long> Save(EngagerMark4.ApplicationCore.Inventory.Entities.Price entity)
        {
            this.repository.BeginTransaction();

            bool needToGenerateSerialNo = entity.Id == 0;
            bool needToGenerateSerialForViceVersa = false;


            this.repository.Save(entity);

            EngagerMark4.ApplicationCore.Inventory.Entities.Price viceVersaPriceEntity = null;

            if (entity.ViceVersa && entity.PickUpPointId != 0 && entity.DropPointId != 0)
            {
                viceVersaPriceEntity = await this.FindByPickUpAndDropOff(entity.DropPointId, entity.PickUpPointId, entity.CustomerId.Value);

                if (viceVersaPriceEntity == null)
                {
                    needToGenerateSerialForViceVersa = true;

                    viceVersaPriceEntity = new EngagerMark4.ApplicationCore.Inventory.Entities.Price()
                    {
                        Name = entity.Name,
                        CustomerId = entity.CustomerId,
                        GLCodeId = entity.GLCodeId,
                        IsTaxable = entity.IsTaxable,
                        AssignedPrice = entity.AssignedPrice,
                        PickUpPoint = entity.DropPoint,
                        PickUpPointId = entity.DropPointId,
                        DropPoint = entity.PickUpPoint,
                        DropPointId = entity.PickUpPointId,
                        DiscountPercent = entity.DiscountPercent,
                        DiscountAmt = entity.DiscountAmt,
                        MaxPax = entity.MaxPax,
                        ExceedAmt = entity.ExceedAmt,
                        StartTime = entity.StartTime,
                        StartTimeBinding = entity.StartTimeBinding,
                        EndTime = entity.EndTime,
                        EndTimeBinding = entity.EndTimeBinding,
                        ViceVersa = entity.ViceVersa
                    };

                    this.repository.Save(viceVersaPriceEntity);
                }
            }

            await this.repository.SaveChangesAsync();

            if (needToGenerateSerialNo)
            {
                SerialNoRepository<PriceSerialNo> serialNoRepoitory = new SerialNoRepository<PriceSerialNo>(_context);

                SystemSettingRepository settingsRepository = new SystemSettingRepository(_context);

                #region SystemSettingCri
                SystemSettingCri sysCri = new SystemSettingCri()
                {
                    StringCris = new Dictionary<string, StringValue>()
                };

                sysCri.StringCris["Code"] = new StringValue { ComparisonOperator = BaseCri.StringComparisonOperator.Equal, Value = AppSettingKey.Key.PRICE_CODE_NUM_COUNT.ToString() };
                #endregion

                var setting = await settingsRepository.GetByCri(sysCri);

                entity.Code = entity.GetPriceCode(serialNoRepoitory.GetSerialNoWithNoTimeConstraint(entity.Id), Convert.ToInt64(setting.FirstOrDefault().Value));

            }

            if (needToGenerateSerialForViceVersa && viceVersaPriceEntity != null)
            {
                SerialNoRepository<PriceSerialNo> serialNoRepoitory = new SerialNoRepository<PriceSerialNo>(_context);

                SystemSettingRepository settingsRepository = new SystemSettingRepository(_context);

                #region SystemSettingCri
                SystemSettingCri sysCri = new SystemSettingCri()
                {
                    StringCris = new Dictionary<string, StringValue>()
                };
                #endregion

                sysCri.StringCris["Code"] = new StringValue { ComparisonOperator = BaseCri.StringComparisonOperator.Equal, Value = AppSettingKey.Key.PRICE_CODE_NUM_COUNT.ToString() };

                var setting = await settingsRepository.GetByCri(sysCri);

                viceVersaPriceEntity.Code = viceVersaPriceEntity.GetPriceCode(serialNoRepoitory.GetSerialNoWithNoTimeConstraint(viceVersaPriceEntity.Id), Convert.ToInt64(setting.FirstOrDefault().Value));

            }

            await this.repository.SaveChangesAsync();

            this.repository.CommitTransaction();

            return entity.Id;
        }

        public async override Task<bool> Delete(EngagerMark4.ApplicationCore.Inventory.Entities.Price entity)
        {
            try
            {
                var isPriceReferenced = this.repository.IsPriceReferenced(entity.Id);

                if (isPriceReferenced) return false;

                var priceLocations = _priceLocationRepository.GetByPriceId(entity.Id).ToList();

                if (priceLocations != null && priceLocations.Count() > 0)
                {
                    foreach (PriceLocation priceLocation in priceLocations)
                    {
                        _priceLocationRepository.Delete(priceLocation);
                    }
                }

                this.repository.Delete(entity);
                await this.repository.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
