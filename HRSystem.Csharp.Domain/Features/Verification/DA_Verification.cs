namespace HRSystem.Csharp.Domain.Features.Verification;

public class DA_Verification
{
    private readonly AppDbContext _db;

    public DA_Verification(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TblVerification>> GetAllAsync()
    {
        return await _db.TblVerifications
            .Where(x => x.DeleteFlag == false)
            .ToListAsync();
    }

    public async Task<TblVerification?> GetByIdAsync(string id)
    {
        return await _db.TblVerifications
            .FirstOrDefaultAsync(x => x.VerificationId == id && x.DeleteFlag == false);
    }

    public async Task<List<TblVerification>> GetByEmailAsync(string email)
    {
        return await _db.TblVerifications
            .Where(x => x.DeleteFlag == false && x.Email == email)
            .ToListAsync();
    }

    public async Task<TblVerification?> GetActiveByEmailAsync(string email)
    {
        return await _db.TblVerifications
            .Where(x => x.DeleteFlag == false && x.IsUsed == false && x.Email == email)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> AddAsync(TblVerification entity)
    {
        await _db.TblVerifications.AddAsync(entity);
        var result = await _db.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateAsync(TblVerification entity)
    {
        _db.TblVerifications.Update(entity);
        var result = await _db.SaveChangesAsync();
        return result > 0;
    }
}